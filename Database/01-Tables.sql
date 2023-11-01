CREATE TABLE IF NOT EXISTS public.organizations
( 
    organization_id bigserial PRIMARY KEY,
    name character varying(64) NOT NULL,
    active boolean NOT NULL DEFAULT TRUE,
    created_on timestamptz NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_on timestamptz NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS public.users
(
    user_id bigserial PRIMARY KEY,
	organization_id INT NOT NULL,
    email character varying(128) NOT NULL UNIQUE,
	first_name character varying(64) NOT NULL,
    last_name character varying(64) NOT NULL,
    title character varying(64),
    profile_photo character varying NULL,
	active boolean NOT NULL DEFAULT TRUE,
    created_on timestamptz NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_on timestamptz NOT NULL DEFAULT CURRENT_TIMESTAMP
);

 CREATE TABLE IF NOT EXISTS public.groups
(
    group_id bigserial PRIMARY KEY,
    organization_id bigint NOT NULL,
	name character varying(64) NOT NULL,
    active boolean NOT NULL DEFAULT TRUE,
    created_on timestamptz NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_on timestamptz NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS public.memberships
(
    group_id bigint NOT NULL,
    user_id bigint NOT NULL,
	created_on timestamptz DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS public.channels
(
    channel_id bigserial PRIMARY KEY,
	group_id bigint,
    name character varying(64) NOT NULL,
    created_on timestamptz NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_on timestamptz NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IF NOT EXISTS public.messages
(
    message_id bigserial PRIMARY KEY,
    sender_id bigint,
	channel_id bigint,
	recipient_id bigint,
    message character varying(512) NOT NULL,
    created_on timestamptz NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_on timestamptz NOT NULL DEFAULT CURRENT_TIMESTAMP,
	is_deleted boolean NOT NULL DEFAULT FALSE
);
