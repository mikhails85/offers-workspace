CREATE TABLE Employees
(
  Id integer NOT NULL AUTO_INCREMENT,
  Name varchar(50) NOT NULL,
  JobTitle varchar(100) NOT NULL,
  Photo varchar(100) NOT NULL,
  CV varchar(100) NOT NULL,
  CONSTRAINT PK_Employees PRIMARY KEY (Id)
);

CREATE TABLE Offers
(
  Id integer NOT NULL AUTO_INCREMENT,
  Name varchar(100) NOT NULL,
  Description varchar(2000) NOT NULL,
  CONSTRAINT PK_Offers PRIMARY KEY (Id)
);

CREATE TABLE OffersSkills
(
  Id integer NOT NULL AUTO_INCREMENT,
  SkillId integer NOT NULL,
  OfferId integer NOT NULL,
  CONSTRAINT PK_OffersSkills PRIMARY KEY (Id)
);

CREATE TABLE Projects
(
  Id integer NOT NULL AUTO_INCREMENT,
  EmployeeId integer NOT NULL,
  Name varchar(256) NOT NULL,
  Description varchar(1000) NOT NULL,
  CONSTRAINT PK_Projects PRIMARY KEY (Id)
);

CREATE TABLE ProjectsSkills
(
  Id integer NOT NULL AUTO_INCREMENT,
  ProjectId integer NOT NULL,
  SkillId integer,
  CONSTRAINT PK_ProjectsSkills PRIMARY KEY (Id)
);

CREATE TABLE Skills
(
  Id integer NOT NULL AUTO_INCREMENT,
  Name varchar(50) NOT NULL,
  CONSTRAINT PK_Skills PRIMARY KEY (Id)
);

ALTER TABLE OffersSkills ADD CONSTRAINT FK_OffersSkills_Offers
  FOREIGN KEY (OfferId) REFERENCES Offers (Id);

ALTER TABLE OffersSkills ADD CONSTRAINT FK_OffersSkills_Skills
  FOREIGN KEY (SkillId) REFERENCES Skills (Id);

ALTER TABLE Projects ADD CONSTRAINT FK_Projects_Employees
  FOREIGN KEY (EmployeeId) REFERENCES Employees (Id);

ALTER TABLE ProjectsSkills ADD CONSTRAINT FK_ProjectsSkills_Projects
  FOREIGN KEY (ProjectId) REFERENCES Projects (Id);

ALTER TABLE ProjectsSkills ADD CONSTRAINT FK_ProjectsSkills_Skills
  FOREIGN KEY (SkillId) REFERENCES Skills (Id);

