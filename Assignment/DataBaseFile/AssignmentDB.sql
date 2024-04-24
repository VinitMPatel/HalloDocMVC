toc.dat                                                                                             0000600 0004000 0002000 00000010077 14612203743 0014446 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        PGDMP   /    5                |         
   Assignment    16.1    16.1     �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false         �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false         �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false         �           1262    106501 
   Assignment    DATABASE        CREATE DATABASE "Assignment" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'English_India.1252';
    DROP DATABASE "Assignment";
                postgres    false         �            1259    106508    Course    TABLE     h   CREATE TABLE public."Course" (
    "Id" integer NOT NULL,
    "Name" character varying(500) NOT NULL
);
    DROP TABLE public."Course";
       public         heap    postgres    false         �            1259    106507    Course_Id_seq    SEQUENCE     �   ALTER TABLE public."Course" ALTER COLUMN "Id" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Course_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    216         �            1259    106514    Student    TABLE     [  CREATE TABLE public."Student" (
    "Id" integer NOT NULL,
    "FirstName" character varying(500) NOT NULL,
    "LastName" character varying(500),
    "CourseId" integer,
    "Age" integer,
    "Email" character varying(500) NOT NULL,
    "Gender" character varying(128),
    "Course" character varying(500),
    "Grade" character varying(128)
);
    DROP TABLE public."Student";
       public         heap    postgres    false         �            1259    106513    Student_Id_seq    SEQUENCE     �   ALTER TABLE public."Student" ALTER COLUMN "Id" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Student_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    218         �          0    106508    Course 
   TABLE DATA           0   COPY public."Course" ("Id", "Name") FROM stdin;
    public          postgres    false    216       4843.dat �          0    106514    Student 
   TABLE DATA           {   COPY public."Student" ("Id", "FirstName", "LastName", "CourseId", "Age", "Email", "Gender", "Course", "Grade") FROM stdin;
    public          postgres    false    218       4845.dat �           0    0    Course_Id_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('public."Course_Id_seq"', 5, true);
          public          postgres    false    215         �           0    0    Student_Id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public."Student_Id_seq"', 11, true);
          public          postgres    false    217         W           2606    106512    Course Course_pkey 
   CONSTRAINT     V   ALTER TABLE ONLY public."Course"
    ADD CONSTRAINT "Course_pkey" PRIMARY KEY ("Id");
 @   ALTER TABLE ONLY public."Course" DROP CONSTRAINT "Course_pkey";
       public            postgres    false    216         Y           2606    106520    Student Student_pkey 
   CONSTRAINT     X   ALTER TABLE ONLY public."Student"
    ADD CONSTRAINT "Student_pkey" PRIMARY KEY ("Id");
 B   ALTER TABLE ONLY public."Student" DROP CONSTRAINT "Student_pkey";
       public            postgres    false    218         Z           2606    106521    Student Student_CourseId_fkey    FK CONSTRAINT     �   ALTER TABLE ONLY public."Student"
    ADD CONSTRAINT "Student_CourseId_fkey" FOREIGN KEY ("CourseId") REFERENCES public."Course"("Id") NOT VALID;
 K   ALTER TABLE ONLY public."Student" DROP CONSTRAINT "Student_CourseId_fkey";
       public          postgres    false    218    4695    216                                                                                                                                                                                                                                                                                                                                                                                                                                                                         4843.dat                                                                                            0000600 0004000 0002000 00000000062 14612203743 0014254 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        1	English
2	Hindi
3	Social
4	Science
5	Maths
\.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                              4845.dat                                                                                            0000600 0004000 0002000 00000000513 14612203743 0014257 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        7	vinit	patel	\N	\N	vinit2273@gmail.com	\N	hindi	\N
8	Khushi	Patel	\N	\N	khu123@gmail.com	\N	eng	\N
9	vijay	solanki	\N	\N	vijay123@gmail.com	Male	Hindi	\N
10	vrushti	Patel	\N	\N	vrushti123@gmail.com	Female	English	\N
11	dfzs	dfv	\N	\N	sky2346@gmail.com	Male	Hindi	\N
5	deepppp	patell	1	21	vin22@gmail.com	Male	English	1st-3rd
\.


                                                                                                                                                                                     restore.sql                                                                                         0000600 0004000 0002000 00000007737 14612203743 0015404 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        --
-- NOTE:
--
-- File paths need to be edited. Search for $$PATH$$ and
-- replace it with the path to the directory containing
-- the extracted data files.
--
--
-- PostgreSQL database dump
--

-- Dumped from database version 16.1
-- Dumped by pg_dump version 16.1

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

DROP DATABASE "Assignment";
--
-- Name: Assignment; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE "Assignment" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'English_India.1252';


ALTER DATABASE "Assignment" OWNER TO postgres;

\connect "Assignment"

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
-- Name: Course; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Course" (
    "Id" integer NOT NULL,
    "Name" character varying(500) NOT NULL
);


ALTER TABLE public."Course" OWNER TO postgres;

--
-- Name: Course_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."Course" ALTER COLUMN "Id" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Course_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Name: Student; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Student" (
    "Id" integer NOT NULL,
    "FirstName" character varying(500) NOT NULL,
    "LastName" character varying(500),
    "CourseId" integer,
    "Age" integer,
    "Email" character varying(500) NOT NULL,
    "Gender" character varying(128),
    "Course" character varying(500),
    "Grade" character varying(128)
);


ALTER TABLE public."Student" OWNER TO postgres;

--
-- Name: Student_Id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

ALTER TABLE public."Student" ALTER COLUMN "Id" ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public."Student_Id_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- Data for Name: Course; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Course" ("Id", "Name") FROM stdin;
\.
COPY public."Course" ("Id", "Name") FROM '$$PATH$$/4843.dat';

--
-- Data for Name: Student; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Student" ("Id", "FirstName", "LastName", "CourseId", "Age", "Email", "Gender", "Course", "Grade") FROM stdin;
\.
COPY public."Student" ("Id", "FirstName", "LastName", "CourseId", "Age", "Email", "Gender", "Course", "Grade") FROM '$$PATH$$/4845.dat';

--
-- Name: Course_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Course_Id_seq"', 5, true);


--
-- Name: Student_Id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public."Student_Id_seq"', 11, true);


--
-- Name: Course Course_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Course"
    ADD CONSTRAINT "Course_pkey" PRIMARY KEY ("Id");


--
-- Name: Student Student_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Student"
    ADD CONSTRAINT "Student_pkey" PRIMARY KEY ("Id");


--
-- Name: Student Student_CourseId_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Student"
    ADD CONSTRAINT "Student_CourseId_fkey" FOREIGN KEY ("CourseId") REFERENCES public."Course"("Id") NOT VALID;


--
-- PostgreSQL database dump complete
--

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 