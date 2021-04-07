DROP TABLE IF EXISTS users CASCADE;
DROP TABLE IF EXISTS sets CASCADE;
DROP TABLE IF EXISTS cards CASCADE;
DROP TABLE IF EXISTS active_sets CASCADE;
DROP TABLE IF EXISTS card_status;


-- CREATE EXTENSION pgcrypto;



CREATE TABLE users(
	id				SERIAL UNIQUE,
	username			TEXT UNIQUE NOT NULL,
	email				TEXT UNIQUE NOT NULL,
	name				TEXT,
	surname			TEXT,
	password			BYTEA NOT NULL,
	email_confirmed		BOOLEAN NOT NULL DEFAULT false,
	active				BOOLEAN NOT NULL DEFAULT true,
	
	PRIMARY KEY(id)
	-- ADD CONSTRAINT email 
);


CREATE TABLE sets(
	id 				SERIAL UNIQUE,
	name 				TEXT UNIQUE NOT NULL,
	creator_id 			INTEGER NOT NULL,
	owner_id 			INTEGER NOT NULL,
	created_date 			TIMESTAMP NOT NULL,
	last_modification		TIMESTAMP NOT NULL,
	is_public			BOOLEAN NOT NULL,

	PRIMARY KEY(id),
	CONSTRAINT fk_creator
		FOREIGN KEY(creator_id)
			REFERENCES users(id)
);


CREATE TABLE cards(
	id 					SERIAL UNIQUE,
	question			TEXT NOT NULL,
	answer				TEXT NOT NULL,
	picture				BYTEA,
	in_set				INTEGER NOT NULL,

	PRIMARY KEY(id),
	CONSTRAINT set_id
		FOREIGN KEY(in_set)
			REFERENCES sets(id)
);


CREATE TABLE active_sets(
	user_id				INTEGER NOT NULL,
	set_id				INTEGER NOT NULL,

	CONSTRAINT id_user
		FOREIGN KEY(user_id)
			REFERENCES users(id),
	CONSTRAINT id_set
		FOREIGN KEY(set_id)
			REFERENCES sets(id)
);


CREATE TABLE card_status(
	card_id				INTEGER NOT NULL,
	user_id				INTEGER NOT NULL,
	last_review			TIMESTAMP NOT NULL,
	next_review			TIMESTAMP NOT NULL,
	active				BOOL NOT NULL,
	difficult			BOOL NOT NULL,

	CONSTRAINT id_card
		FOREIGN KEY(card_id)
			REFERENCES cards(id),
	CONSTRAINT id_user
		FOREIGN KEY(user_id)
			REFERENCES users(id)
);
-- CREATE EXTENSION pgcrypto;

ALTER user flashcards_app with encrypted password 'fc_app';
GRANT ALL PRIVILEGES ON TABLE users, sets, cards, active_sets, card_status TO flashcards_app;
-- GRANT ALL PRIVILEGES ON DATABASE flashcards to flashcards_app;
GRANT USAGE ON ALL SEQUENCES IN SCHEMA public TO flashcards_app;
GRANT ALL PRIVILEGES ON SCHEMA public TO flashcards_app;
-- GRANT ALL PRIVILEGES ON DATABASE flashcards TO flashcards_app;
