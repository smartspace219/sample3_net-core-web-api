--
-- PostgreSQL database dump
--

-- Dumped from database version 16.2
-- Dumped by pg_dump version 16.2

-- Started on 2024-03-25 14:38:35

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

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 221 (class 1259 OID 24695)
-- Name: log_data; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.log_data (
    id bigint NOT NULL,
    serial character(4) NOT NULL,
    sequence smallint,
    created time without time zone NOT NULL,
    component character(3),
    event character(4),
    short_1 character(10) DEFAULT '0'::bpchar,
    short_2 character(10) DEFAULT '0'::bpchar,
    information text
);


ALTER TABLE public.log_data OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 24694)
-- Name: log_data_id_seq1; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.log_data_id_seq1
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.log_data_id_seq1 OWNER TO postgres;

--
-- TOC entry 4799 (class 0 OID 0)
-- Dependencies: 220
-- Name: log_data_id_seq1; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.log_data_id_seq1 OWNED BY public.log_data.id;


--
-- TOC entry 4644 (class 2604 OID 24698)
-- Name: log_data id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.log_data ALTER COLUMN id SET DEFAULT nextval('public.log_data_id_seq1'::regclass);


--
-- TOC entry 4793 (class 0 OID 24695)
-- Dependencies: 221
-- Data for Name: log_data; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.log_data (id, serial, sequence, created, component, event, short_1, short_2, information) FROM stdin;
\.


--
-- TOC entry 4800 (class 0 OID 0)
-- Dependencies: 220
-- Name: log_data_id_seq1; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.log_data_id_seq1', 1, false);


--
-- TOC entry 4648 (class 2606 OID 24704)
-- Name: log_data log_data_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.log_data
    ADD CONSTRAINT log_data_pkey PRIMARY KEY (id);


-- Completed on 2024-03-25 14:38:36

--
-- PostgreSQL database dump complete
--

