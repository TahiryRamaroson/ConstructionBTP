\c postgres

drop database construction;

create database construction;

\c construction

create table Utilisateur(
    id serial primary key,
    nom varchar(100),
    prenom varchar(100),
    naissance date,
    numTel varchar(20),
    email varchar(100),
    mdp varchar(64),
    role varchar(10) default 'CLIENT'
);

create table Unite(
    id serial primary key,
    nom varchar(100)
);

create table Maison(
    id serial primary key,
    nom varchar(100),
    description text,
    duree double precision
);
ALTER TABLE Maison add column surface double precision default 0;

create table MaisonDuree(
    idMaison int,
    duree double precision,
    dateinsertion timestamp,
    foreign key(idMaison) references Maison(id)
);

create table Finition(
    id serial primary key,
    nom varchar(100),
    taux double precision
);

create table FinitionTaux(
    idFinition int,
    taux double precision,
    dateinsertion timestamp,
    foreign key(idFinition) references Finition(id)
);

create table Travaux(
    id serial primary key,
    numero varchar(20),
    nom varchar(100),
    idUnite int,
    prixUnitaire double precision,
    foreign key(idUnite) references Unite(id)
);

create table TravauxTarif(
    idTravaux int,
    prixUnitaire double precision,
    dateinsertion timestamp,
    foreign key(idTravaux) references Travaux(id)
);

create table TravauxMaison(
    idMaison int,
    idTravaux int,
    quantite double precision,
    dateinsertion timestamp,
    foreign key(idMaison) references Maison(id),
    foreign key(idTravaux) references Travaux(id)
);

create table Devis(
    id serial primary key,
    numero varchar(20),
    idClient int,
    idMaison int,
    nomMaison varchar(100),
    montantTravaux double precision,
    tauxFinition double precision,
    nomFinition varchar(100),
    debutTravaux timestamp,
    dateCreation date,
    lieu varchar(100),
    foreign key(idClient) references Utilisateur(id),
    foreign key(idMaison) references Maison(id)
);

create table DevisDetails(
    idDevis int,
    idTravaux int,
    quantite double precision,
    prixUnitaire double precision,
    foreign key(idDevis) references Devis(id),
    foreign key(idTravaux) references Travaux(id) 
);

create table Paiement(
    id serial primary key,
    montant double precision,
    idDevis int,
    datePaiement timestamp,
    numero varchar(100),
    foreign key(idDevis) references Devis(id)
);


insert into Utilisateur values
    (default, 'RAMAROSON', 'Tahiry', '2003-01-24', '0321111111', 'Tahiry@gmail.com', 'ffc74d3d2725b7ad3c9f4c8b59a9dfcbde445dbf6909e04b024106217fca1062', 'ADMIN'),
    (default, null, null, null, '0323333333', null, null, 'CLIENT');

insert into Unite values
    (default, 'm2'),
    (default, 'm3');

insert into TypeTravaux values
    (default, '0001', 'Preparatoire'),
    (default, '0002', 'Terrassement'),
    (default, '0003', 'Infrastructure');

insert into Maison values
    (default,'Villa', 'moderne et chaleureux', 35),
    (default,'Cabane', 'Conviviale', 17.5),
    (default,'Appartement', 'spacieux et sécurisé', 24);

insert into MaisonDuree values
    (1, 35, '2024-05-13'),
    (2, 17.5, '2024-05-13'),
    (3, 24, '2024-05-13');

insert into Finition values
    (default,'Standard', 0),
    (default,'Gold', 5),
    (default,'Premium', 10),
    (default,'VIP', 20);

insert into FinitionTaux values
    (1, 0, '2024-05-13'),
    (2, 5, '2024-05-13'),
    (3, 10, '2024-05-13'),
    (4, 20, '2024-05-13');

insert into Travaux values
    (default, '0001', 'Dressage du plateforme', 1, 2, 8000),
    (default, '0002', 'Mur de soutenement', 2, 1, 11500),
    (default, '0003', 'Remblai technique', 2, 3, 7200),
    (default, '0004', 'Décapage des terrains meuble', 1, 2, 15000),
    (default, '0005', 'Chape', 2, 3, 11000);

insert into TravauxTarif values
    (1, 8000, '2024-05-13'),
    (2, 11500, '2024-05-13'),
    (3, 7200, '2024-05-13'),
    (4, 15000, '2024-05-13'),
    (5, 11000, '2024-05-13');

-- maison, travaux, quantite, dateinsertion
insert into TravauxMaison values
    (1, 1, 24, '2024-05-13'),
    (1, 5, 59.4, '2024-05-13'),
    (1, 2, 41, '2024-05-13'),

    (2, 3, 17, '2024-05-13'),
    (2, 1, 33.6, '2024-05-13'),

    (3, 1, 47, '2024-05-13'),
    (3, 2, 101.6, '2024-05-13'),
    (3, 4, 69, '2024-05-13'),
    (3, 5, 58, '2024-05-13');

-- id, numero, client, maison, nommaison, montantTravaux, tauxFinition, nomfinition, debutTravaux, dateCreation 
insert into Devis values
    (default, '0001', 2, 1, 'Villa', 1229400, 10, 'Premium', '2024-05-16', '2024-05-13', 'Analakely'),
    (default, '0002', 2, 3, 'Appartement', 3171200, 15, 'VIP', '2024-05-17', '2024-05-13', 'Analakely');

-- devis, travaux, quantite, prixunitaire
insert into DevisDetails values
    (1, 1, 22, 8000),   -- og: 24, 8000
    (1, 5, 59.4, 11000),    -- og: 59.4, 11000
    (1, 2, 40, 10000),  -- og: 41, 11500

    (2, 1, 47, 8000),   -- og: 47, 8000
    (2, 2, 95.2, 11000),    -- og: 101.6, 11500
    (2, 4, 74, 15000),  -- og: 69, 15000
    (2, 5, 58, 11000);  -- og: 58, 11000

-- id, montant, devis, datepaiement
insert into Paiement values
    (default, 120000, 1, '14-05-2024'),
    (default, 171200, 2, '14-05-2024'),
    (default, 89400, 1, '15-05-2024');

create or replace view V_devisDetails as
select DevisDetails.*, Travaux.nom as nomTravaux, Travaux.idunite
from DevisDetails
join Travaux on DevisDetails.idTravaux = Travaux.id; 


create or replace view V_devisDetails_affichage as
select V_devisDetails.idDevis, V_devisDetails.quantite, V_devisDetails.prixUnitaire, V_devisDetails.nomTravaux, Unite.nom as nomUnite
from V_devisDetails
join Unite on V_devisDetails.idunite = Unite.id;


SELECT d.*, (d.montantTravaux + d.montantTravaux * d.tauxFinition / 100) as montantTotal
FROM Devis d
WHERE (d.montantTravaux + d.montantTravaux * d.tauxFinition / 100) > (
    SELECT COALESCE(SUM(p.montant), 0)
    FROM Paiement p
    WHERE p.idDevis = d.id
);


create or replace view V_devisEnCours as
SELECT d.*, COALESCE(SUM(p.montant), 0) AS totalPaiement, (d.montantTravaux + d.montantTravaux * d.tauxFinition / 100) as montantTotal
FROM Devis d
LEFT JOIN Paiement p ON d.id = p.idDevis
GROUP BY d.id
HAVING (d.montantTravaux + d.montantTravaux * d.tauxFinition / 100) > COALESCE(SUM(p.montant), 0);


create or replace view V_devisAdmin as
SELECT d.*, COALESCE(SUM(p.montant), 0) AS totalPaiement, (d.montantTravaux + d.montantTravaux * d.tauxFinition / 100) as montantTotal,
(COALESCE(SUM(p.montant), 0)/(d.montantTravaux + d.montantTravaux * d.tauxFinition / 100)*100) as pourcentagePaiement
FROM Devis d
LEFT JOIN Paiement p ON d.id = p.idDevis
GROUP BY d.id;


create or replace view V_devisAdmin_Affichage as
select V_devisAdmin.*, Utilisateur.numTel
from V_devisAdmin
join Utilisateur on V_devisAdmin.idClient = Utilisateur.id;


create or replace view V_devisEnCours_Affichage as
select V_devisEnCours.*, Utilisateur.numTel
from V_devisEnCours
join Utilisateur on V_devisEnCours.idClient = Utilisateur.id;


create or replace view V_dureeRecent as
SELECT DISTINCT ON (idMaison)
    idMaison,
    duree,
    dateinsertion
FROM MaisonDuree
ORDER BY idMaison, dateinsertion DESC;


create or replace view V_tarifRecent as
SELECT DISTINCT ON (idTravaux)
    idTravaux,
    prixUnitaire,
    dateinsertion
FROM TravauxTarif
ORDER BY idTravaux, dateinsertion DESC;


create or replace view V_tauxRecent as
SELECT DISTINCT ON (idFinition)
    idFinition,
    taux,
    dateinsertion
FROM FinitionTaux
ORDER BY idFinition, dateinsertion DESC;


create or replace view V_FinitionRecent as
select * from Finition;



create or replace view V_maisonRecent as
select * from Maison;


create or replace view V_travauxRecent as
select * from Travaux;


create or replace view V_montantMaison as
select sum(TravMai.quantite * TravMai.prixUnitaire) as sommeTravaux, TravMai.idMaison
from
(select TravauxMaison.*, V_travauxRecent.prixUnitaire
from V_travauxRecent
join TravauxMaison on V_travauxRecent.id = TravauxMaison.idTravaux) as TravMai
group by TravMai.idMaison;


create or replace view V_maison_Affichage as
select V_maisonRecent.*, V_montantMaison.sommeTravaux
from V_maisonRecent
join V_montantMaison on V_maisonRecent.id = V_montantMaison.idMaison;

create or replace view V_travaux_Affichage as
select Travaux.*, Unite.nom as nomUnite
from Travaux
join Unite on Travaux.idunite = unite.id;


-- WITH months AS (
--     SELECT generate_series(1, 12) AS month
-- )
-- SELECT
--     months.month,
--     '2023'AS year,
--     COALESCE(SUM(d.montantTravaux + d.montantTravaux * d.tauxFinition / 100), 0) AS montant_total
-- FROM
--     months
-- LEFT JOIN
--     Devis d ON months.month = EXTRACT(MONTH FROM d.dateCreation) AND EXTRACT(YEAR FROM d.dateCreation) = '2023'
-- GROUP BY
--     months.month
-- ORDER BY
--     months.month;


ALTER TABLE Maison add column surface double precision default 0;
ALTER TABLE Devis add column lieu varchar(100);
ALTER TABLE Utilisateur ALTER COLUMN role SET DEFAULT 'CLIENT';

ALTER TABLE Paiement add



create table MaisonTravauxCsv(
    type_maison varchar(100),
    description text,
    surface varchar(100),
    code_travaux varchar(100),
    type_travaux varchar(100),
    unite varchar(50),
    prixunitaire varchar(50),
    quantite varchar(50),
    duree_travaux varchar(50)
);

create table DevisCsv(
    client varchar(100),
    ref_devis varchar(100),
    type_maison varchar(100),
    finition varchar(100),
    taux_finition varchar(100),
    date_devis varchar(20),
    date_debut varchar(20),
    lieu varchar(100)
);

create table PaiementCsv(
    ref_devis varchar(100),
    ref_paiement varchar(100),
    date_paiement varchar(100),
    montant varchar(100)
);


INSERT INTO Paiement (montant, idDevis, datePaiement, numero)
SELECT CAST(montant AS double precision), Devis.id, TO_TIMESTAMP(date_paiement, 'YYYY-MM-DD'), ref_paiement
FROM PaiementCsv
JOIN Devis on PaiementCsv.ref_devis = Devis.numero;


--Insérer Finition
INSERT INTO Finition (nom, taux)
SELECT distinct finition, CAST(taux_finition AS double precision)
FROM DevisCsv;


-- Insérer les données dans la table Devis
INSERT INTO Devis (numero, idClient, idMaison, nomMaison, tauxFinition, nomFinition, debutTravaux, dateCreation)
SELECT ref_devis, (SELECT id FROM Utilisateur WHERE numTel = client), (SELECT id FROM Maison WHERE nom = type_maison), type_maison, CAST(taux_finition AS double precision), finition, TO_TIMESTAMP(date_debut, 'YYYY-MM-DD'), TO_DATE(date_devis, 'YYYY-MM-DD')
FROM DevisCsv;

-- Insérer les données dans la table DevisDetails
INSERT INTO DevisDetails (idDevis, idTravaux, quantite, prixUnitaire)
SELECT (SELECT id FROM Devis WHERE numero = ref_devis), idTravaux, quantite, (SELECT prixUnitaire FROM Travaux WHERE id = idTravaux)
FROM DevisCsv, TravauxMaison
WHERE DevisCsv.type_maison = (SELECT nom FROM Maison WHERE id = TravauxMaison.idMaison);


UPDATE Devis
SET montantTravaux = (
    SELECT SUM(quantite * prixUnitaire)
    FROM DevisDetails
    WHERE Devis.id = DevisDetails.idDevis
)
WHERE EXISTS (
    SELECT 1
    FROM DevisDetails
    WHERE Devis.id = DevisDetails.idDevis
);
