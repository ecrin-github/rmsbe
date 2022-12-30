--
-- PostgreSQL database dump
--

-- Dumped from database version 13.3
-- Dumped by pg_dump version 14.4

-- Started on 2022-12-30 18:27:18

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 4 (class 2615 OID 17183)
-- Name: rms; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA rms;


ALTER SCHEMA rms OWNER TO postgres;

--
-- TOC entry 406 (class 1255 OID 22055)
-- Name: make_audit_trigger(character varying, character varying); Type: PROCEDURE; Schema: rms; Owner: postgres
--

CREATE PROCEDURE rms.make_audit_trigger(schema_name character varying, table_name character varying)
    LANGUAGE plpgsql
    AS $_$
DECLARE audit_function_create_string VARCHAR:= 'CREATE OR REPLACE FUNCTION '||schema_name||'.'||table_name||'_audit_trigger_func()
   RETURNS trigger AS $$
   BEGIN
   if (TG_OP = ''INSERT'') then
       INSERT INTO '||schema_name||'.record_changes (
           table_name, table_id, 
		   change_type, change_time, user_name,
           prior, post
       )
       VALUES(
		   '''||table_name||''', NEW.id,
		   2, CURRENT_TIMESTAMP, NEW.last_edited_by,
		   null, to_jsonb(NEW)
       );
       RETURN NEW;
   elsif (TG_OP = ''UPDATE'') then
       INSERT INTO '||schema_name||'.record_changes (
           table_name, table_id, 
		   change_type, change_time, user_name,
           prior, post
       )
       VALUES(
		   '''||table_name||''', NEW.id,
		   1, CURRENT_TIMESTAMP, NEW.last_edited_by,
		   to_jsonb(OLD), to_jsonb(NEW)
       );
       RETURN NEW;
   elsif (TG_OP = ''DELETE'') then
       INSERT INTO '||schema_name||'.record_changes (
           table_name, table_id, 
		   change_type, change_time, user_name,
           prior, post
       )
	   VALUES(
		   '''||table_name||''', OLD.id,
		   -1, CURRENT_TIMESTAMP, OLD.last_edited_by,
		   to_jsonb(OLD), null
       );
       RETURN OLD;
   end if;
     
   END;
   $$
   LANGUAGE plpgsql;';
   
audit_function_add_string VARCHAR:= 'DROP TRIGGER IF EXISTS '||table_name||'_audit_trigger ON '||schema_name||'.'||table_name||';
CREATE TRIGGER '||table_name||'_audit_trigger
AFTER INSERT OR UPDATE OR DELETE ON '||schema_name||'.'||table_name||'
FOR EACH ROW EXECUTE FUNCTION '||schema_name||'.'||table_name||'_audit_trigger_func();';

BEGIN

-- store the generated strings
insert into rms.trigger_code(table_name, trigger_code, trigger_application) 
VALUES (table_name, audit_function_create_string, audit_function_add_string);

END;
$_$;


ALTER PROCEDURE rms.make_audit_trigger(schema_name character varying, table_name character varying) OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 257 (class 1259 OID 22315)
-- Name: access_prereq_types; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.access_prereq_types (
    id integer NOT NULL,
    name character varying NOT NULL,
    description character varying,
    list_order integer,
    created_on timestamp with time zone DEFAULT CURRENT_TIMESTAMP(0)
);


ALTER TABLE rms.access_prereq_types OWNER TO postgres;

--
-- TOC entry 254 (class 1259 OID 22288)
-- Name: check_status_types; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.check_status_types (
    id integer NOT NULL,
    name character varying NOT NULL,
    description character varying,
    list_order integer,
    created_on timestamp with time zone DEFAULT CURRENT_TIMESTAMP(0)
);


ALTER TABLE rms.check_status_types OWNER TO postgres;

--
-- TOC entry 296 (class 1259 OID 26522)
-- Name: dtas; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.dtas (
    id integer NOT NULL,
    dtp_id integer NOT NULL,
    conforms_to_default boolean,
    variations character varying,
    dta_file_path character varying,
    repo_signatory_1 integer,
    repo_signatory_2 integer,
    provider_signatory_1 integer,
    provider_signatory_2 integer,
    notes character varying,
    created_on timestamp with time zone DEFAULT CURRENT_TIMESTAMP(0)
);


ALTER TABLE rms.dtas OWNER TO postgres;

--
-- TOC entry 295 (class 1259 OID 26520)
-- Name: dtas_id_seq; Type: SEQUENCE; Schema: rms; Owner: postgres
--

ALTER TABLE rms.dtas ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME rms.dtas_id_seq
    START WITH 100001
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 300 (class 1259 OID 26547)
-- Name: dtp_datasets; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.dtp_datasets (
    id integer NOT NULL,
    dtp_id integer NOT NULL,
    sd_oid character varying,
    legal_status_id integer,
    legal_status_text character varying,
    legal_status_path character varying,
    desc_md_check_status_id integer DEFAULT 11,
    desc_md_check_date date,
    desc_md_check_by integer,
    deident_check_status_id integer DEFAULT 11,
    deident_check_date date,
    deident_check_by integer,
    notes character varying,
    created_on timestamp with time zone DEFAULT CURRENT_TIMESTAMP(0)
);


ALTER TABLE rms.dtp_datasets OWNER TO postgres;

--
-- TOC entry 299 (class 1259 OID 26545)
-- Name: dtp_datasets_id_seq; Type: SEQUENCE; Schema: rms; Owner: postgres
--

ALTER TABLE rms.dtp_datasets ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME rms.dtp_datasets_id_seq
    START WITH 100001
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 260 (class 1259 OID 24587)
-- Name: dtp_notes; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.dtp_notes (
    id integer NOT NULL,
    dtp_id integer NOT NULL,
    text character varying,
    author integer,
    created_on timestamp with time zone DEFAULT CURRENT_TIMESTAMP(0)
);


ALTER TABLE rms.dtp_notes OWNER TO postgres;

--
-- TOC entry 259 (class 1259 OID 24585)
-- Name: dtp_notes_id_seq; Type: SEQUENCE; Schema: rms; Owner: postgres
--

ALTER TABLE rms.dtp_notes ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME rms.dtp_notes_id_seq
    START WITH 100001
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 304 (class 1259 OID 26593)
-- Name: dtp_objects; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.dtp_objects (
    id integer NOT NULL,
    dtp_id integer NOT NULL,
    sd_oid character varying,
    is_dataset boolean DEFAULT false,
    access_type_id integer,
    download_allowed boolean DEFAULT true,
    access_details character varying,
    embargo_requested boolean DEFAULT false,
    embargo_regime character varying,
    embargo_still_applies boolean DEFAULT false,
    access_check_status_id integer DEFAULT 11,
    access_check_date date,
    access_check_by integer,
    md_check_status_id integer DEFAULT 11,
    md_check_date date,
    md_check_by integer,
    notes character varying,
    created_on timestamp with time zone DEFAULT CURRENT_TIMESTAMP(0)
);


ALTER TABLE rms.dtp_objects OWNER TO postgres;

--
-- TOC entry 292 (class 1259 OID 26492)
-- Name: xdtp_objects; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.xdtp_objects (
    id integer NOT NULL,
    dtp_id integer NOT NULL,
    sd_oid character varying,
    is_dataset boolean DEFAULT false,
    access_type_id integer,
    download_allowed boolean DEFAULT true,
    access_details character varying,
    requires_embargo_period boolean DEFAULT false,
    embargo_end_date date,
    embargo_still_applies boolean DEFAULT false,
    access_check_status_id integer DEFAULT 11,
    access_check_date date,
    access_check_by integer,
    md_check_status_id integer DEFAULT 11,
    md_check_date date,
    md_check_by integer,
    notes character varying,
    created_on timestamp with time zone DEFAULT CURRENT_TIMESTAMP(0)
);


ALTER TABLE rms.xdtp_objects OWNER TO postgres;

--
-- TOC entry 291 (class 1259 OID 26490)
-- Name: dtp_objects_id_seq; Type: SEQUENCE; Schema: rms; Owner: postgres
--

ALTER TABLE rms.xdtp_objects ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME rms.dtp_objects_id_seq
    START WITH 100001
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 303 (class 1259 OID 26591)
-- Name: dtp_objects_id_seq1; Type: SEQUENCE; Schema: rms; Owner: postgres
--

ALTER TABLE rms.dtp_objects ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME rms.dtp_objects_id_seq1
    START WITH 100001
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 264 (class 1259 OID 24611)
-- Name: dtp_people; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.dtp_people (
    id integer NOT NULL,
    dtp_id integer NOT NULL,
    person_id integer NOT NULL,
    notes character varying,
    created_on timestamp with time zone DEFAULT CURRENT_TIMESTAMP(0)
);


ALTER TABLE rms.dtp_people OWNER TO postgres;

--
-- TOC entry 263 (class 1259 OID 24609)
-- Name: dtp_people_id_seq; Type: SEQUENCE; Schema: rms; Owner: postgres
--

ALTER TABLE rms.dtp_people ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME rms.dtp_people_id_seq
    START WITH 100001
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 270 (class 1259 OID 24647)
-- Name: dtp_prereqs; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.dtp_prereqs (
    id integer NOT NULL,
    dtp_id integer NOT NULL,
    sd_oid character varying,
    pre_requisite_type_id integer,
    pre_requisite_notes character varying,
    created_on timestamp with time zone DEFAULT CURRENT_TIMESTAMP(0)
);


ALTER TABLE rms.dtp_prereqs OWNER TO postgres;

--
-- TOC entry 269 (class 1259 OID 24645)
-- Name: dtp_prereqs_id_seq; Type: SEQUENCE; Schema: rms; Owner: postgres
--

ALTER TABLE rms.dtp_prereqs ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME rms.dtp_prereqs_id_seq
    START WITH 100001
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 252 (class 1259 OID 22270)
-- Name: dtp_status_types; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.dtp_status_types (
    id integer NOT NULL,
    name character varying NOT NULL,
    description character varying,
    list_order integer,
    created_on timestamp with time zone DEFAULT CURRENT_TIMESTAMP(0)
);


ALTER TABLE rms.dtp_status_types OWNER TO postgres;

--
-- TOC entry 290 (class 1259 OID 26479)
-- Name: dtp_studies; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.dtp_studies (
    id integer NOT NULL,
    dtp_id integer NOT NULL,
    sd_sid character varying,
    md_check_status_id integer DEFAULT 11,
    md_check_date date,
    md_check_by integer,
    created_on timestamp with time zone DEFAULT CURRENT_TIMESTAMP(0)
);


ALTER TABLE rms.dtp_studies OWNER TO postgres;

--
-- TOC entry 289 (class 1259 OID 26477)
-- Name: dtp_studies_id_seq; Type: SEQUENCE; Schema: rms; Owner: postgres
--

ALTER TABLE rms.dtp_studies ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME rms.dtp_studies_id_seq
    START WITH 100001
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 249 (class 1259 OID 22081)
-- Name: dtps; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.dtps (
    id integer NOT NULL,
    org_id integer NOT NULL,
    display_name character varying,
    status_id integer DEFAULT 11 NOT NULL,
    initial_contact_date date,
    set_up_completed date,
    md_access_granted date,
    md_complete_date date,
    dta_agreed_date date,
    upload_access_requested date,
    upload_access_confirmed date,
    uploads_complete date,
    qc_checks_completed date,
    md_integrated_with_mdr date,
    availability_requested date,
    availability_confirmed date,
    created_on timestamp with time zone DEFAULT CURRENT_TIMESTAMP(0)
);


ALTER TABLE rms.dtps OWNER TO postgres;

--
-- TOC entry 248 (class 1259 OID 22079)
-- Name: dtps_id_seq; Type: SEQUENCE; Schema: rms; Owner: postgres
--

ALTER TABLE rms.dtps ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME rms.dtps_id_seq
    START WITH 500001
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 298 (class 1259 OID 26534)
-- Name: duas; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.duas (
    id integer NOT NULL,
    dup_id integer NOT NULL,
    conforms_to_default boolean,
    variations character varying,
    repo_is_proxy_provider boolean DEFAULT true,
    dua_file_path character varying,
    repo_signatory_1 integer,
    repo_signatory_2 integer,
    provider_signatory_1 integer,
    provider_signatory_2 integer,
    requester_signatory_1 integer,
    requester_signatory_2 integer,
    notes character varying,
    created_on timestamp with time zone DEFAULT CURRENT_TIMESTAMP(0)
);


ALTER TABLE rms.duas OWNER TO postgres;

--
-- TOC entry 297 (class 1259 OID 26532)
-- Name: duas_id_seq; Type: SEQUENCE; Schema: rms; Owner: postgres
--

ALTER TABLE rms.duas ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME rms.duas_id_seq
    START WITH 100001
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 262 (class 1259 OID 24599)
-- Name: dup_notes; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.dup_notes (
    id integer NOT NULL,
    dup_id integer NOT NULL,
    text character varying,
    author integer,
    created_on timestamp with time zone DEFAULT CURRENT_TIMESTAMP(0)
);


ALTER TABLE rms.dup_notes OWNER TO postgres;

--
-- TOC entry 261 (class 1259 OID 24597)
-- Name: dup_notes_id_seq; Type: SEQUENCE; Schema: rms; Owner: postgres
--

ALTER TABLE rms.dup_notes ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME rms.dup_notes_id_seq
    START WITH 100001
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 294 (class 1259 OID 26510)
-- Name: dup_objects; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.dup_objects (
    id integer NOT NULL,
    dup_id integer NOT NULL,
    sd_oid character varying,
    access_type_id integer,
    access_details character varying,
    notes character varying,
    created_on timestamp with time zone DEFAULT CURRENT_TIMESTAMP(0)
);


ALTER TABLE rms.dup_objects OWNER TO postgres;

--
-- TOC entry 293 (class 1259 OID 26508)
-- Name: dup_objects_id_seq; Type: SEQUENCE; Schema: rms; Owner: postgres
--

ALTER TABLE rms.dup_objects ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME rms.dup_objects_id_seq
    START WITH 100001
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 266 (class 1259 OID 24623)
-- Name: dup_people; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.dup_people (
    id integer NOT NULL,
    dup_id integer NOT NULL,
    person_id integer NOT NULL,
    notes character varying,
    created_on timestamp with time zone DEFAULT CURRENT_TIMESTAMP(0)
);


ALTER TABLE rms.dup_people OWNER TO postgres;

--
-- TOC entry 265 (class 1259 OID 24621)
-- Name: dup_people_id_seq; Type: SEQUENCE; Schema: rms; Owner: postgres
--

ALTER TABLE rms.dup_people ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME rms.dup_people_id_seq
    START WITH 100001
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 272 (class 1259 OID 24660)
-- Name: dup_prereqs; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.dup_prereqs (
    id integer NOT NULL,
    dup_id integer NOT NULL,
    sd_oid character varying,
    pre_requisite_id integer,
    pre_requisite_notes character varying,
    pre_requisite_met date,
    met_notes character varying,
    created_on timestamp with time zone DEFAULT CURRENT_TIMESTAMP(0)
);


ALTER TABLE rms.dup_prereqs OWNER TO postgres;

--
-- TOC entry 271 (class 1259 OID 24658)
-- Name: dup_prereqs_id_seq; Type: SEQUENCE; Schema: rms; Owner: postgres
--

ALTER TABLE rms.dup_prereqs ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME rms.dup_prereqs_id_seq
    START WITH 100001
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 302 (class 1259 OID 26562)
-- Name: dup_sec_use; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.dup_sec_use (
    id integer NOT NULL,
    dup_id integer NOT NULL,
    secondary_use_summary character varying,
    publication character varying,
    doi character varying,
    attribution_present boolean,
    notes character varying,
    created_on timestamp with time zone DEFAULT CURRENT_TIMESTAMP(0)
);


ALTER TABLE rms.dup_sec_use OWNER TO postgres;

--
-- TOC entry 301 (class 1259 OID 26560)
-- Name: dup_sec_use_id_seq; Type: SEQUENCE; Schema: rms; Owner: postgres
--

ALTER TABLE rms.dup_sec_use ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME rms.dup_sec_use_id_seq
    START WITH 100001
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 253 (class 1259 OID 22279)
-- Name: dup_status_types; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.dup_status_types (
    id integer NOT NULL,
    name character varying NOT NULL,
    description character varying,
    list_order integer,
    created_on timestamp with time zone DEFAULT CURRENT_TIMESTAMP(0)
);


ALTER TABLE rms.dup_status_types OWNER TO postgres;

--
-- TOC entry 268 (class 1259 OID 24635)
-- Name: dup_studies; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.dup_studies (
    id integer NOT NULL,
    dup_id integer NOT NULL,
    sd_sid character varying,
    created_on timestamp with time zone DEFAULT CURRENT_TIMESTAMP(0)
);


ALTER TABLE rms.dup_studies OWNER TO postgres;

--
-- TOC entry 267 (class 1259 OID 24633)
-- Name: dup_studies_id_seq; Type: SEQUENCE; Schema: rms; Owner: postgres
--

ALTER TABLE rms.dup_studies ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME rms.dup_studies_id_seq
    START WITH 100001
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 251 (class 1259 OID 22095)
-- Name: dups; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.dups (
    id integer NOT NULL,
    org_id integer NOT NULL,
    display_name character varying,
    status_id integer NOT NULL,
    initial_contact_date date,
    set_up_completed date,
    prereqs_met date,
    dua_agreed_date date,
    availability_requested date,
    availability_confirmed date,
    access_confirmed date,
    created_on timestamp with time zone DEFAULT CURRENT_TIMESTAMP(0)
);


ALTER TABLE rms.dups OWNER TO postgres;

--
-- TOC entry 250 (class 1259 OID 22093)
-- Name: dups_id_seq; Type: SEQUENCE; Schema: rms; Owner: postgres
--

ALTER TABLE rms.dups ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME rms.dups_id_seq
    START WITH 500001
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 256 (class 1259 OID 22306)
-- Name: legal_status_types; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.legal_status_types (
    id integer NOT NULL,
    name character varying NOT NULL,
    description character varying,
    list_order integer,
    created_on timestamp with time zone DEFAULT CURRENT_TIMESTAMP(0)
);


ALTER TABLE rms.legal_status_types OWNER TO postgres;

--
-- TOC entry 285 (class 1259 OID 25825)
-- Name: people; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.people (
    id integer NOT NULL,
    title character varying,
    given_name character varying,
    family_name character varying,
    designation character varying,
    org_id integer,
    org_name character varying,
    email character varying,
    comments character varying,
    created_on timestamp with time zone DEFAULT CURRENT_TIMESTAMP(0)
);


ALTER TABLE rms.people OWNER TO postgres;

--
-- TOC entry 284 (class 1259 OID 25823)
-- Name: people_id_seq; Type: SEQUENCE; Schema: rms; Owner: postgres
--

ALTER TABLE rms.people ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME rms.people_id_seq
    START WITH 400001
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 287 (class 1259 OID 25837)
-- Name: people_roles; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.people_roles (
    id integer NOT NULL,
    person_id integer NOT NULL,
    role_id integer NOT NULL,
    role_name character varying,
    is_current boolean,
    granted timestamp with time zone,
    revoked timestamp with time zone,
    created_on timestamp with time zone DEFAULT CURRENT_TIMESTAMP(0)
);


ALTER TABLE rms.people_roles OWNER TO postgres;

--
-- TOC entry 286 (class 1259 OID 25835)
-- Name: people_roles_id_seq; Type: SEQUENCE; Schema: rms; Owner: postgres
--

ALTER TABLE rms.people_roles ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME rms.people_roles_id_seq
    START WITH 300001
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 255 (class 1259 OID 22297)
-- Name: repo_access_types; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.repo_access_types (
    id integer NOT NULL,
    name character varying NOT NULL,
    description character varying,
    list_order integer,
    created_on timestamp with time zone DEFAULT CURRENT_TIMESTAMP(0)
);


ALTER TABLE rms.repo_access_types OWNER TO postgres;

--
-- TOC entry 288 (class 1259 OID 26174)
-- Name: tempcorr; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.tempcorr (
    sd_oid character varying,
    new_sd_oid text,
    identifier_value character varying
);


ALTER TABLE rms.tempcorr OWNER TO postgres;

--
-- TOC entry 283 (class 1259 OID 25814)
-- Name: test_data; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.test_data (
    id integer NOT NULL,
    table_name character varying,
    id_in_table integer,
    comments character varying,
    created_on timestamp with time zone DEFAULT CURRENT_TIMESTAMP(2)
);


ALTER TABLE rms.test_data OWNER TO postgres;

--
-- TOC entry 282 (class 1259 OID 25812)
-- Name: test_data_id_seq; Type: SEQUENCE; Schema: rms; Owner: postgres
--

ALTER TABLE rms.test_data ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME rms.test_data_id_seq
    START WITH 100001
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 247 (class 1259 OID 22058)
-- Name: trigger_code; Type: TABLE; Schema: rms; Owner: postgres
--

CREATE TABLE rms.trigger_code (
    id integer NOT NULL,
    table_name character varying,
    trigger_code character varying,
    trigger_application character varying,
    rec_count integer
);


ALTER TABLE rms.trigger_code OWNER TO postgres;

--
-- TOC entry 246 (class 1259 OID 22056)
-- Name: trigger_code_id_seq; Type: SEQUENCE; Schema: rms; Owner: postgres
--

ALTER TABLE rms.trigger_code ALTER COLUMN id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME rms.trigger_code_id_seq
    START WITH 11
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 3592 (class 2606 OID 22323)
-- Name: access_prereq_types access_prereq_types_pkey; Type: CONSTRAINT; Schema: rms; Owner: postgres
--

ALTER TABLE ONLY rms.access_prereq_types
    ADD CONSTRAINT access_prereq_types_pkey PRIMARY KEY (id);


--
-- TOC entry 3586 (class 2606 OID 22296)
-- Name: check_status_types check_status_types_pkey; Type: CONSTRAINT; Schema: rms; Owner: postgres
--

ALTER TABLE ONLY rms.check_status_types
    ADD CONSTRAINT check_status_types_pkey PRIMARY KEY (id);


--
-- TOC entry 3635 (class 2606 OID 26530)
-- Name: dtas dtas_pkey; Type: CONSTRAINT; Schema: rms; Owner: postgres
--

ALTER TABLE ONLY rms.dtas
    ADD CONSTRAINT dtas_pkey PRIMARY KEY (id);


--
-- TOC entry 3649 (class 2606 OID 26557)
-- Name: dtp_datasets dtp_datasets_pkey; Type: CONSTRAINT; Schema: rms; Owner: postgres
--

ALTER TABLE ONLY rms.dtp_datasets
    ADD CONSTRAINT dtp_datasets_pkey PRIMARY KEY (id);


--
-- TOC entry 3595 (class 2606 OID 24595)
-- Name: dtp_notes dtp_notes_pkey; Type: CONSTRAINT; Schema: rms; Owner: postgres
--

ALTER TABLE ONLY rms.dtp_notes
    ADD CONSTRAINT dtp_notes_pkey PRIMARY KEY (id);


--
-- TOC entry 3629 (class 2606 OID 26506)
-- Name: xdtp_objects dtp_objects_pkey; Type: CONSTRAINT; Schema: rms; Owner: postgres
--

ALTER TABLE ONLY rms.xdtp_objects
    ADD CONSTRAINT dtp_objects_pkey PRIMARY KEY (id);


--
-- TOC entry 3656 (class 2606 OID 26607)
-- Name: dtp_objects dtp_objects_pkey1; Type: CONSTRAINT; Schema: rms; Owner: postgres
--

ALTER TABLE ONLY rms.dtp_objects
    ADD CONSTRAINT dtp_objects_pkey1 PRIMARY KEY (id);


--
-- TOC entry 3601 (class 2606 OID 24619)
-- Name: dtp_people dtp_people_pkey; Type: CONSTRAINT; Schema: rms; Owner: postgres
--

ALTER TABLE ONLY rms.dtp_people
    ADD CONSTRAINT dtp_people_pkey PRIMARY KEY (id);


--
-- TOC entry 3610 (class 2606 OID 24655)
-- Name: dtp_prereqs dtp_prereqs_pkey; Type: CONSTRAINT; Schema: rms; Owner: postgres
--

ALTER TABLE ONLY rms.dtp_prereqs
    ADD CONSTRAINT dtp_prereqs_pkey PRIMARY KEY (id);


--
-- TOC entry 3582 (class 2606 OID 22278)
-- Name: dtp_status_types dtp_status_types_pkey; Type: CONSTRAINT; Schema: rms; Owner: postgres
--

ALTER TABLE ONLY rms.dtp_status_types
    ADD CONSTRAINT dtp_status_types_pkey PRIMARY KEY (id);


--
-- TOC entry 3626 (class 2606 OID 26488)
-- Name: dtp_studies dtp_studies_pkey; Type: CONSTRAINT; Schema: rms; Owner: postgres
--

ALTER TABLE ONLY rms.dtp_studies
    ADD CONSTRAINT dtp_studies_pkey PRIMARY KEY (id);


--
-- TOC entry 3577 (class 2606 OID 22090)
-- Name: dtps dtps_pkey; Type: CONSTRAINT; Schema: rms; Owner: postgres
--

ALTER TABLE ONLY rms.dtps
    ADD CONSTRAINT dtps_pkey PRIMARY KEY (id);


--
-- TOC entry 3640 (class 2606 OID 26543)
-- Name: duas duas_pkey; Type: CONSTRAINT; Schema: rms; Owner: postgres
--

ALTER TABLE ONLY rms.duas
    ADD CONSTRAINT duas_pkey PRIMARY KEY (id);


--
-- TOC entry 3598 (class 2606 OID 24607)
-- Name: dup_notes dup_notes_pkey; Type: CONSTRAINT; Schema: rms; Owner: postgres
--

ALTER TABLE ONLY rms.dup_notes
    ADD CONSTRAINT dup_notes_pkey PRIMARY KEY (id);


--
-- TOC entry 3632 (class 2606 OID 26518)
-- Name: dup_objects dup_objects_pkey; Type: CONSTRAINT; Schema: rms; Owner: postgres
--

ALTER TABLE ONLY rms.dup_objects
    ADD CONSTRAINT dup_objects_pkey PRIMARY KEY (id);


--
-- TOC entry 3604 (class 2606 OID 24631)
-- Name: dup_people dup_people_pkey; Type: CONSTRAINT; Schema: rms; Owner: postgres
--

ALTER TABLE ONLY rms.dup_people
    ADD CONSTRAINT dup_people_pkey PRIMARY KEY (id);


--
-- TOC entry 3614 (class 2606 OID 24668)
-- Name: dup_prereqs dup_prereqs_pkey; Type: CONSTRAINT; Schema: rms; Owner: postgres
--

ALTER TABLE ONLY rms.dup_prereqs
    ADD CONSTRAINT dup_prereqs_pkey PRIMARY KEY (id);


--
-- TOC entry 3653 (class 2606 OID 26570)
-- Name: dup_sec_use dup_sec_use_pkey; Type: CONSTRAINT; Schema: rms; Owner: postgres
--

ALTER TABLE ONLY rms.dup_sec_use
    ADD CONSTRAINT dup_sec_use_pkey PRIMARY KEY (id);


--
-- TOC entry 3584 (class 2606 OID 22287)
-- Name: dup_status_types dup_status_types_pkey; Type: CONSTRAINT; Schema: rms; Owner: postgres
--

ALTER TABLE ONLY rms.dup_status_types
    ADD CONSTRAINT dup_status_types_pkey PRIMARY KEY (id);


--
-- TOC entry 3607 (class 2606 OID 24643)
-- Name: dup_studies dup_studies_pkey; Type: CONSTRAINT; Schema: rms; Owner: postgres
--

ALTER TABLE ONLY rms.dup_studies
    ADD CONSTRAINT dup_studies_pkey PRIMARY KEY (id);


--
-- TOC entry 3580 (class 2606 OID 22103)
-- Name: dups dups_pkey; Type: CONSTRAINT; Schema: rms; Owner: postgres
--

ALTER TABLE ONLY rms.dups
    ADD CONSTRAINT dups_pkey PRIMARY KEY (id);


--
-- TOC entry 3590 (class 2606 OID 22314)
-- Name: legal_status_types legal_status_types_pkey; Type: CONSTRAINT; Schema: rms; Owner: postgres
--

ALTER TABLE ONLY rms.legal_status_types
    ADD CONSTRAINT legal_status_types_pkey PRIMARY KEY (id);


--
-- TOC entry 3620 (class 2606 OID 25833)
-- Name: people people_pkey; Type: CONSTRAINT; Schema: rms; Owner: postgres
--

ALTER TABLE ONLY rms.people
    ADD CONSTRAINT people_pkey PRIMARY KEY (id);


--
-- TOC entry 3623 (class 2606 OID 25845)
-- Name: people_roles people_roles_pkey; Type: CONSTRAINT; Schema: rms; Owner: postgres
--

ALTER TABLE ONLY rms.people_roles
    ADD CONSTRAINT people_roles_pkey PRIMARY KEY (id);


--
-- TOC entry 3588 (class 2606 OID 22305)
-- Name: repo_access_types repo_access_types_pkey; Type: CONSTRAINT; Schema: rms; Owner: postgres
--

ALTER TABLE ONLY rms.repo_access_types
    ADD CONSTRAINT repo_access_types_pkey PRIMARY KEY (id);


--
-- TOC entry 3617 (class 2606 OID 25822)
-- Name: test_data test_data_pkey; Type: CONSTRAINT; Schema: rms; Owner: postgres
--

ALTER TABLE ONLY rms.test_data
    ADD CONSTRAINT test_data_pkey PRIMARY KEY (id);


--
-- TOC entry 3574 (class 2606 OID 22065)
-- Name: trigger_code trigger_code_pkey; Type: CONSTRAINT; Schema: rms; Owner: postgres
--

ALTER TABLE ONLY rms.trigger_code
    ADD CONSTRAINT trigger_code_pkey PRIMARY KEY (id);


--
-- TOC entry 3633 (class 1259 OID 26531)
-- Name: dtas_dtp_id; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX dtas_dtp_id ON rms.dtas USING btree (dtp_id);


--
-- TOC entry 3647 (class 1259 OID 26558)
-- Name: dtp_datasets_dtp_id; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX dtp_datasets_dtp_id ON rms.dtp_datasets USING btree (dtp_id);


--
-- TOC entry 3650 (class 1259 OID 26559)
-- Name: dtp_datasets_sd_oid; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX dtp_datasets_sd_oid ON rms.dtp_datasets USING btree (sd_oid);


--
-- TOC entry 3593 (class 1259 OID 24596)
-- Name: dtp_notes_dtp_id; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX dtp_notes_dtp_id ON rms.dtp_notes USING btree (dtp_id);


--
-- TOC entry 3654 (class 1259 OID 26608)
-- Name: dtp_objects_dtp2_id; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX dtp_objects_dtp2_id ON rms.dtp_objects USING btree (dtp_id);


--
-- TOC entry 3627 (class 1259 OID 26507)
-- Name: dtp_objects_dtp_id; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX dtp_objects_dtp_id ON rms.xdtp_objects USING btree (dtp_id);


--
-- TOC entry 3599 (class 1259 OID 24620)
-- Name: dtp_people_dtp_id; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX dtp_people_dtp_id ON rms.dtp_people USING btree (dtp_id);


--
-- TOC entry 3608 (class 1259 OID 24657)
-- Name: dtp_prereqs_dtp_id; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX dtp_prereqs_dtp_id ON rms.dtp_prereqs USING btree (dtp_id);


--
-- TOC entry 3611 (class 1259 OID 24656)
-- Name: dtp_prereqs_sd_oid; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX dtp_prereqs_sd_oid ON rms.dtp_prereqs USING btree (sd_oid);


--
-- TOC entry 3624 (class 1259 OID 26489)
-- Name: dtp_studies_dtp_id; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX dtp_studies_dtp_id ON rms.dtp_studies USING btree (dtp_id);


--
-- TOC entry 3575 (class 1259 OID 22091)
-- Name: dtps_org_id; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX dtps_org_id ON rms.dtps USING btree (org_id);


--
-- TOC entry 3638 (class 1259 OID 26544)
-- Name: duas_dup_id; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX duas_dup_id ON rms.duas USING btree (dup_id);


--
-- TOC entry 3596 (class 1259 OID 24608)
-- Name: dup_notes_dup_id; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX dup_notes_dup_id ON rms.dup_notes USING btree (dup_id);


--
-- TOC entry 3630 (class 1259 OID 26519)
-- Name: dup_objects_dup_id; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX dup_objects_dup_id ON rms.dup_objects USING btree (dup_id);


--
-- TOC entry 3602 (class 1259 OID 24632)
-- Name: dup_people_dup_id; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX dup_people_dup_id ON rms.dup_people USING btree (dup_id);


--
-- TOC entry 3612 (class 1259 OID 24670)
-- Name: dup_prereqs_dup_id; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX dup_prereqs_dup_id ON rms.dup_prereqs USING btree (dup_id);


--
-- TOC entry 3615 (class 1259 OID 24669)
-- Name: dup_prereqs_sd_oid; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX dup_prereqs_sd_oid ON rms.dup_prereqs USING btree (sd_oid);


--
-- TOC entry 3651 (class 1259 OID 26571)
-- Name: dup_sec_use_dup_id; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX dup_sec_use_dup_id ON rms.dup_sec_use USING btree (dup_id);


--
-- TOC entry 3605 (class 1259 OID 24644)
-- Name: dup_studies_dup_id; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX dup_studies_dup_id ON rms.dup_studies USING btree (dup_id);


--
-- TOC entry 3578 (class 1259 OID 22104)
-- Name: dups_org_id; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX dups_org_id ON rms.dups USING btree (org_id);


--
-- TOC entry 3636 (class 1259 OID 27131)
-- Name: i_dtas_sign1; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX i_dtas_sign1 ON rms.dtas USING btree (repo_signatory_1);


--
-- TOC entry 3637 (class 1259 OID 27132)
-- Name: i_dtas_sign2; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX i_dtas_sign2 ON rms.dtas USING btree (repo_signatory_2);


--
-- TOC entry 3641 (class 1259 OID 27135)
-- Name: i_duas_psign1; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX i_duas_psign1 ON rms.duas USING btree (provider_signatory_1);


--
-- TOC entry 3642 (class 1259 OID 27136)
-- Name: i_duas_psign2; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX i_duas_psign2 ON rms.duas USING btree (provider_signatory_2);


--
-- TOC entry 3643 (class 1259 OID 27137)
-- Name: i_duas_rsign1; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX i_duas_rsign1 ON rms.duas USING btree (requester_signatory_1);


--
-- TOC entry 3644 (class 1259 OID 27138)
-- Name: i_duas_rsign2; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX i_duas_rsign2 ON rms.duas USING btree (requester_signatory_2);


--
-- TOC entry 3645 (class 1259 OID 27133)
-- Name: i_duas_sign1; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX i_duas_sign1 ON rms.duas USING btree (repo_signatory_1);


--
-- TOC entry 3646 (class 1259 OID 27134)
-- Name: i_duas_sign2; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX i_duas_sign2 ON rms.duas USING btree (repo_signatory_2);


--
-- TOC entry 3618 (class 1259 OID 25834)
-- Name: people_org_id; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX people_org_id ON rms.people USING btree (org_id);


--
-- TOC entry 3621 (class 1259 OID 25846)
-- Name: people_role_person_id; Type: INDEX; Schema: rms; Owner: postgres
--

CREATE INDEX people_role_person_id ON rms.people_roles USING btree (person_id);


-- Completed on 2022-12-30 18:27:18

--
-- PostgreSQL database dump complete
--

