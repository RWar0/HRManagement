CREATE DATABASE HRManagementProject;
GO

USE HRManagementProject;
GO

CREATE TABLE Salaries (
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	Description NVARCHAR(MAX) NULL,
	BruttoPrice MONEY NOT NULL,
	TaxRate FLOAT NOT NULL,
	ZusTaxRate FLOAT NOT NULL,
	Declusions MONEY NOT NULL,
	IsActive BIT NOT NULL,
	CreationDateTime DATETIME NOT NULL,
	EditDateTime DATETIME NULL,
	DeleteDateTime DATETIME NULL,
);

CREATE TABLE Positions (
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	Title NVARCHAR(255) NOT NULL,
	DepartmentName NVARCHAR(255) NULL,
	IsActive BIT NOT NULL,
	CreationDateTime DATETIME NOT NULL,
	EditDateTime DATETIME NULL,
	DeleteDateTime DATETIME NULL,
);

CREATE TABLE Employees (
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	Firstname NVARCHAR(60) NOT NULL,
	Surname NVARCHAR(80) NOT NULL,
	Gender NVARCHAR(15) NOT NULL,
	EmploymentType NVARCHAR(100) NOT NULL,
	SalaryId INT NOT NULL FOREIGN KEY REFERENCES Salaries(Id),
	PositionId INT NOT NULL FOREIGN KEY REFERENCES Positions(Id),
	IsActive BIT NOT NULL,
	CreationDateTime DATETIME NOT NULL,
	EditDateTime DATETIME NULL,
	DeleteDateTime DATETIME NULL,
);

CREATE TABLE Adresses (
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	Country NVARCHAR(128) NOT NULL,
	City NVARCHAR(128) NOT NULL,
	PostalCode NVARCHAR(16) NULL,
	Street NVARCHAR(128) NULL,
	HouseNumber NVARCHAR(16) NOT NULL,
	FlatNumber NVARCHAR(16) NULL,
	IsActive BIT NOT NULL,
	CreationDateTime DATETIME NOT NULL,
	EditDateTime DATETIME NULL,
	DeleteDateTime DATETIME NULL,
);

CREATE TABLE PersonalData (
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	EmployeeId INT NOT NULL FOREIGN KEY REFERENCES Employees(Id),
	PESEL CHAR(11) NOT NULL,
	PhoneNumber VARCHAR(16) NOT NULL, -- +48 123 456 789
	DateOfBirth DATE NOT NULL,
	PlaceOfBirth NVARCHAR(60) NOT NULL,
	ChildrenQuantity INT NOT NULL,
	Education NVARCHAR(40) NOT NULL,
	ResidenceAdressId INT NOT NULL FOREIGN KEY REFERENCES Adresses(Id),
	RegistrationAdressId INT NOT NULL FOREIGN KEY REFERENCES Adresses(Id),
	IsActive BIT NOT NULL,
	CreationDateTime DATETIME NOT NULL,
	EditDateTime DATETIME NULL,
	DeleteDateTime DATETIME NULL,
);

CREATE TABLE Careers (
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	Title NVARCHAR(255) NOT NULL,
	Position NVARCHAR(255) NOT NULL,
	BeginDate DATE NOT NULL,
	EndDate DATE NOT NULL,
	IsActive BIT NOT NULL,
	CreationDateTime DATETIME NOT NULL,
	EditDateTime DATETIME NULL,
	DeleteDateTime DATETIME NULL,
);

CREATE TABLE EmployeeCareer (
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	EmployeeId INT NOT NULL FOREIGN KEY REFERENCES Employees(Id),
	CareerId INT NOT NULL FOREIGN KEY REFERENCES Careers(Id),
	IsActive BIT NOT NULL,
	CreationDateTime DATETIME NOT NULL,
	EditDateTime DATETIME NULL,
	DeleteDateTime DATETIME NULL,
);

CREATE TABLE Skills (
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	Title NVARCHAR(MAX) NOT NULL,
	IsActive BIT NOT NULL,
	CreationDateTime DATETIME NOT NULL,
	EditDateTime DATETIME NULL,
	DeleteDateTime DATETIME NULL,
);

CREATE TABLE EmployeSkills (
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	EmployeeId INT NOT NULL FOREIGN KEY REFERENCES Employees(Id),
	SkillId INT NOT NULL FOREIGN KEY REFERENCES Skills(Id),
	IsActive BIT NOT NULL,
	CreationDateTime DATETIME NOT NULL,
	EditDateTime DATETIME NULL,
	DeleteDateTime DATETIME NULL,
);

CREATE TABLE LeaveTypes (
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	Title NVARCHAR(MAX) NOT NULL,
	Description NVARCHAR(MAX) NULL,
	IsActive BIT NOT NULL,
	CreationDateTime DATETIME NOT NULL,
	EditDateTime DATETIME NULL,
	DeleteDateTime DATETIME NULL,
);

CREATE TABLE LeaveStatuses (
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	Title NVARCHAR(MAX) NOT NULL,
	IsActive BIT NOT NULL,
	CreationDateTime DATETIME NOT NULL,
	EditDateTime DATETIME NULL,
	DeleteDateTime DATETIME NULL,
);

CREATE TABLE Leaves (
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	EmployeeId INT NOT NULL FOREIGN KEY REFERENCES Employees(Id),
	LeaveTypeId INT NOT NULL FOREIGN KEY REFERENCES LeaveTypes(Id),
	Reason NVARCHAR(MAX) NULL,
	BeginDate DATE NOT NULL,
	EndDate DATE NOT NULL,
	LeaveStatusId INT NOT NULL FOREIGN KEY REFERENCES LeaveStatuses(Id),
	IsActive BIT NOT NULL,
	CreationDateTime DATETIME NOT NULL,
	EditDateTime DATETIME NULL,
	DeleteDateTime DATETIME NULL,
);

CREATE TABLE Promotions (
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	EmployeeId INT NOT NULL FOREIGN KEY REFERENCES Employees(Id),
	PromotionDate DATE NOT NULL,
	OldPositionId INT NOT NULL FOREIGN KEY REFERENCES Positions(Id),
	NewPositionId INT NOT NULL FOREIGN KEY REFERENCES Positions(Id),
	OldSalaryId INT NOT NULL FOREIGN KEY REFERENCES Salaries(Id),
	NewSalaryId INT NOT NULL FOREIGN KEY REFERENCES Salaries(Id),
	IsActive BIT NOT NULL,
	CreationDateTime DATETIME NOT NULL,
	EditDateTime DATETIME NULL,
	DeleteDateTime DATETIME NULL,
);

CREATE TABLE Trainings (
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	Title NVARCHAR(MAX) NOT NULL,
	Description NVARCHAR(MAX) NOT NULL,
	BeginDate DATE NOT NULL,
	EndDate DATE NOT NULL,
	IsActive BIT NOT NULL,
	CreationDateTime DATETIME NOT NULL,
	EditDateTime DATETIME NULL,
	DeleteDateTime DATETIME NULL,
);

CREATE TABLE EmployeeTrainings (
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	EmployeeId INT NOT NULL FOREIGN KEY REFERENCES Employees(Id),
	TrainingId INT NOT NULL FOREIGN KEY REFERENCES Trainings(Id),
	IsActive BIT NOT NULL,
	CreationDateTime DATETIME NOT NULL,
	EditDateTime DATETIME NULL,
	DeleteDateTime DATETIME NULL,
);

CREATE TABLE Bonuses (
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	Title NVARCHAR(100) NOT NULL,
	Description NVARCHAR(MAX) NULL,
	Price MONEY NOT NULL,
	BeginDate DATE NOT NULL,
	EndDate DATE NOT NULL,
	IsActive BIT NOT NULL,
	CreationDateTime DATETIME NOT NULL,
	EditDateTime DATETIME NULL,
	DeleteDateTime DATETIME NULL,
);

CREATE TABLE EmployeeBonuses (
	Id INT IDENTITY(1, 1) PRIMARY KEY,
	EmployeeId INT NOT NULL FOREIGN KEY REFERENCES Employees(Id),
	BonusId INT NOT NULL FOREIGN KEY REFERENCES Bonuses(Id),
	IsActive BIT NOT NULL,
	CreationDateTime DATETIME NOT NULL,
	EditDateTime DATETIME NULL,
	DeleteDateTime DATETIME NULL,
);