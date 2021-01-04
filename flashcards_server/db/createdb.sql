DROP TABLE IF EXISTS users CASCADE;
DROP TABLE IF EXISTS sets CASCADE;
DROP TABLE IF EXISTS cards CASCADE;
DROP TABLE IF EXISTS active_sets CASCADE;
DROP TABLE IF EXISTS card_status;



CREATE TABLE users(
	id					SERIAL,
	username			TEXT UNIQUE NOT NULL,
	email				TEXT UNIQUE NOT NULL,
	name				TEXT,
	surname				TEXT,
	password			TEXT NOT NULL,
	active 				BOOLEAN NOT NULL DEFAULT true,
	
	PRIMARY KEY(id)
	-- ADD CONSTRAINT email 
);


CREATE TABLE sets(
	id 					SERIAL,
	name 				TEXT UNIQUE NOT NULL,
	creator_id 			INTEGER NOT NULL,
	owner_id 			INTEGER NOT NULL,
	created_date 		TIME NOT NULL,
	last_modification	TIME NOT NULL,
	is_public			BOOLEAN NOT NULL,

	PRIMARY KEY(id),
	CONSTRAINT fk_creator
		FOREIGN KEY(creator_id)
			REFERENCES users(id)
);


CREATE TABLE cards(
	id 					SERIAL,
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
	is_paused			BOOLEAN NOT NULL,

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
	last_review			TIME NOT NULL,
	next_review			TIME NOT NULL,
	active				BOOL NOT NULL,
	difficult			BOOL NOT NULL,

	CONSTRAINT id_card
		FOREIGN KEY(card_id)
			REFERENCES cards(id),
	CONSTRAINT id_user
		FOREIGN KEY(user_id)
			REFERENCES users(id)
);

ALTER user flashcards_app with encrypted password 'fc_app';
GRANT ALL PRIVILEGES ON TABLE users TO flashcards_app;
GRANT ALL PRIVILEGES ON DATABASE flashcards to flashcards_app;
GRANT USAGE ON ALL SEQUENCES IN SCHEMA public TO flashcards_app;