/********************----E-Health-CareManagement-System-----***************/

create database eHealthCare
GO
use eHealthCare;
GO
/*************************Users Table******************************/
create table Users(
	userID int primary key identity(1,1),
	userName varchar(50),
    userType varchar(100),
	Password varchar(100)
);

/****************************Patients Table*************************************/
create table Patients(
	PatientID int primary key identity(1,1),
	FirstName varchar(30), 
    LastName varchar(30), 
    Gender varchar(5),
    ContactNumber varchar(11),
    Age int,
    EmailID varchar(30),
    BloodGroup varchar(5),
    Address varchar(50)
);

/*****************************Doctor Table**********************************/
create table Doctors(
	DoctorID int primary key identity(1,1),
	FirstName varchar(30), 
    LastName varchar(30), 
    Gender varchar(10),
    ContactNumber varchar(11),
    Age int,
    EntryCharge int,
    Qualification varchar(50),
    DoctorType varchar(50),
    EmailId varchar(30)
);

/********************************Appointments Table***************************************/
create table Appointments
(
	AppointmentID int primary key identity(1,1),
    Problem varchar(50),
	PatientID int,
    DoctorName varchar(20),
    DoctorID int,
    DoctorType varchar(20),
    Qualification varchar(20),
    DoctorFees int,
    PaymentStatus varchar(33),
    AppointmentStatus varchar(30),
    CONSTRAINT FKAP FOREIGN KEY (PatientID) REFERENCES Patients(PatientID) ON DELETE NO ACTION,
    CONSTRAINT FKAdocid FOREIGN KEY (DoctorID) REFERENCES Doctors(DoctorID) ON DELETE NO ACTION
);

/*******************************Reports Table**********************************************/
create table Reports
(
	ReportID int primary key identity(1,1),
    AppointmentID int,
    PatientID int,
    DoctorID int,
    MedicinePrescribed varchar(200),
    DoctorComment varchar(200),
    CONSTRAINT FKapid FOREIGN KEY (AppointmentID) REFERENCES Appointments(AppointmentID) ON DELETE NO ACTION,
    CONSTRAINT FKRP FOREIGN KEY (PatientID) REFERENCES Patients(PatientID) ON DELETE NO ACTION,
    CONSTRAINT FKRdocid FOREIGN KEY (DoctorID) REFERENCES Doctors(DoctorID) ON DELETE NO ACTION
);

/******************************Feedback Table************************************************/
create table Feedback
(
	FeedbackID int Primary Key identity(1,1),
	PatientID int,
    Points int,
    DocNature varchar(200),
    Location varchar(200),
    PatientComment varchar(1000),
    CONSTRAINT FKFpid FOREIGN KEY (PatientID) REFERENCES Patients(PatientID) ON DELETE NO ACTION
);
