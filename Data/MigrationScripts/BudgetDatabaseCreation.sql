CREATE TABLE AppInfo (
    InfoKey TEXT PRIMARY KEY,
    InfoValue TEXT NOT NULL
);

INSERT INTO AppInfo (InfoKey, InfoValue)
VALUES
('AppVersion', '0.0'),
('DatabaseVersion', '0.0');

