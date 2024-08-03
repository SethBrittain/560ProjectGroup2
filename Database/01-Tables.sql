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
    profile_photo character varying(128) DEFAULT NULL,
	active boolean NOT NULL DEFAULT TRUE,
	password BYTEA NOT NULL,
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

CREATE TABLE IF NOT EXISTS public.channel_messages
(
    channel_message_id bigserial PRIMARY KEY,
    sender_id bigint NOT NULL,
	channel_id bigint NOT NULL,
    message character varying(512) NOT NULL,
    created_on timestamptz NOT NULL DEFAULT CURRENT_TIMESTAMP,
    updated_on timestamptz NOT NULL DEFAULT CURRENT_TIMESTAMP,
	CONSTRAINT fk_sender FOREIGN KEY (sender_id) REFERENCES users(user_id),
	CONSTRAINT fk_channel FOREIGN KEY (channel_id) REFERENCES channels(channel_id)
);

CREATE TABLE IF NOT EXISTS public.direct_messages
(
	direct_message_id bigserial PRIMARY KEY,
	sender_id bigint NOT NULL,
	recipient_id bigint NOT NULL,
	message character varying(512) NOT NULL,
	created_on timestamptz NOT NULL DEFAULT CURRENT_TIMESTAMP,
	updated_on timestamptz NOT NULL DEFAULT CURRENT_TIMESTAMP,
	CONSTRAINT fk_sender FOREIGN KEY (sender_id) REFERENCES users(user_id),
	CONSTRAINT fk_recipient FOREIGN KEY (recipient_id) REFERENCES users(user_id)
);
