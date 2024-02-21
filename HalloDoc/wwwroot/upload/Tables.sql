-- Table: Admin
CREATE TABLE Admin (
    AdminId INT NOT NULL PRIMARY KEY,
    AspNetUserId character varying(128) NOT NULL,
    FirstName character varying(100) NOT NULL,
    LastName character varying(100),
    Email character varying(50) NOT NULL,
    Mobile character varying(20),
    Address1 character varying(500),
    Address2 character varying(500),
    City character varying(100),
    RegionId INT,
    Zip character varying(10),
    AltPhone character varying(20),
    CreatedBy character varying(128) NOT NULL,
    CreatedDate timestamp without time zone NOT NULL,
    ModifiedBy character varying(128),
    ModifiedDate timestamp without time zone,
    Status smallint,
    IsDeleted BIT,
    RoleId INT
);

-- Table: AdminRegion
CREATE TABLE AdminRegion (
    AdminRegionId INT NOT NULL PRIMARY KEY,
    AdminId INT NOT NULL,
    RegionId INT NOT NULL
);

-- Table: AspNetRoles
CREATE TABLE AspNetRoles (
    Id character varying(128) NOT NULL PRIMARY KEY,
    Name character varying(256) NOT NULL
);

-- Table: AspNetUserRoles
CREATE TABLE AspNetUserRoles (
    UserId character varying(128) NOT NULL,
    RoleId character varying(128) NOT NULL,
    PRIMARY KEY (UserId, RoleId)
);

-- Table: AspNetUsers
CREATE TABLE AspNetUsers (
    Id character varying(128) NOT NULL PRIMARY KEY,
    UserName character varying(256) NOT NULL,
    PasswordHash character varying,
    SecurityStamp character varying,
    Email character varying(256),
    EmailConfirmed BIT NOT NULL,
    PhoneNumber character varying,
    PhoneNumberConfirmed BIT NOT NULL,
    TwoFactorEnabled BIT NOT NULL,
    LockoutEndDateUtc timestamp without time zone,
    LockoutEnabled BIT NOT NULL,
    AccessFailedCount INT NOT NULL,
    IP character varying(20),
    CorePasswordHash character varying,
    HashVersion INT,
    ModifiedDate timestamp without time zone
);

-- Table: BlockRequests
CREATE TABLE BlockRequests (
    BlockRequestId INT NOT NULL PRIMARY KEY,
    PhoneNumber character varying(50),
    Email character varying(50),
    IsActive BIT,
    Reason character varying,
    RequestId character varying(50),
    IP character varying(20),
    CreatedDate timestamp without time zone,
    ModifiedDate timestamp without time zone
);

-- Table: Business
CREATE TABLE Business (
    BusinessId INT NOT NULL PRIMARY KEY,
    Name character varying(100) NOT NULL,
    BusinessTypeId INT,
    Address1 character varying(500),
    Address2 character varying(500),
    City character varying(50),
    RegionId INT,
    ZipCode character varying(10),
    PhoneNumber character varying(20),
    FaxNumber character varying(20),
    IsRegistered BIT,
    CreatedBy character varying(128),
    CreatedDate timestamp without time zone NOT NULL,
    ModifiedBy character varying(128),
    ModifiedDate timestamp without time zone,
    Status smallint,
    IsDeleted BIT,
    IP character varying(20)
);

-- Table: BusinessType
CREATE TABLE BusinessType (
    BusinessTypeId INT NOT NULL PRIMARY KEY,
    Name character varying(50) NOT NULL
);

-- Table: CaseTag
CREATE TABLE CaseTag (
    CaseTagId INT NOT NULL PRIMARY KEY,
    Name character varying(50) NOT NULL
);

-- Table: Concierge
CREATE TABLE Concierge (
    ConciergeId INT NOT NULL PRIMARY KEY,
    ConciergeName character varying(100) NOT NULL,
    Address character varying(150),
    Street character varying(50) NOT NULL,
    City character varying(50) NOT NULL,
    State character varying(50) NOT NULL,
    ZipCode character varying(50) NOT NULL,
    CreatedDate timestamp without time zone NOT NULL,
    RegionId INT NOT NULL,
    IP character varying(20)
);

-- Table: EmailLog
CREATE TABLE EmailLog (
    EmailLogID DECIMAL(9) NOT NULL PRIMARY KEY,
    EmailTemplate character varying NOT NULL,
    SubjectName character varying(200) NOT NULL,
    EmailID character varying(200) NOT NULL,
    ConfirmationNumber character varying(200),
    FilePath character varying,
    RoleId INT,
    RequestId INT,
    AdminId INT,
    PhysicianId INT,
    CreateDate timestamp without time zone NOT NULL,
    SentDate timestamp without time zone,
    IsEmailSent BIT,
    SentTries INT,
    Action INT
);

-- Table: HealthProfessionals
CREATE TABLE HealthProfessionals (
    VendorId INT NOT NULL PRIMARY KEY,
    VendorName character varying(100) NOT NULL,
    Profession INT,
    FaxNumber character varying(50) NOT NULL,
    Address character varying(150),
    City character varying(100),
    State character varying(50),
    Zip character varying(50),
    RegionId INT,
    CreatedDate timestamp without time zone NOT NULL,
    ModifiedDate timestamp without time zone,
    PhoneNumber character varying(100),
    IsDeleted BIT,
    IP character varying(20),
    Email character varying(50),
    BusinessContact character varying(100)
);

-- Table: HealthProfessionalType
CREATE TABLE HealthProfessionalType (
    HealthProfessionalId INT NOT NULL PRIMARY KEY,
    ProfessionName character varying(50) NOT NULL,
    CreatedDate timestamp without time zone NOT NULL,
    IsActive BIT,
    IsDeleted BIT
);

-- Table: Menu
CREATE TABLE Menu (
    MenuId INT NOT NULL PRIMARY KEY,
    Name character varying(50) NOT NULL,
    AccountType smallint NOT NULL,
    SortOrder INT
);

-- Table: OrderDetails
CREATE TABLE OrderDetails (
    Id INT NOT NULL PRIMARY KEY,
    VendorId INT,
    RequestId INT,
    FaxNumber character varying(50),
    Email character varying(50),
    BusinessContact character varying(100),
    Prescription character varying(1000),
    NoOfRefill INT,
    CreatedDate timestamp without time zone,
    CreatedBy character varying(100)
);

-- Table: Physician
CREATE TABLE Physician (
    PhysicianId INT NOT NULL PRIMARY KEY,
    AspNetUserId character varying(128),
    FirstName character varying(100) NOT NULL,
    LastName character varying(100),
    Email character varying(50) NOT NULL,
    Mobile character varying(20),
    MedicalLicense character varying(500),
    Photo character varying(100),
    AdminNotes character varying(500),
    IsAgreementDoc BIT,
    IsBackgroundDoc BIT,
    IsTrainingDoc BIT,
    IsNonDisclosureDoc BIT,
    Address1 character varying(500),
    Address2 character varying(500),
    City character varying(100),
    RegionId INT,
    Zip character varying(10),
    AltPhone character varying(20),
    CreatedBy character varying(128) NOT NULL,
    CreatedDate timestamp without time zone NOT NULL,
    ModifiedBy character varying(128),
    ModifiedDate timestamp without time zone,
    Status smallint,
    BusinessName character varying(100) NOT NULL,
    BusinessWebsite character varying(200) NOT NULL,
    IsDeleted BIT,
    RoleId INT,
    NPINumber character varying(500),
    IsLicenseDoc BIT,
    Signature character varying(100),
    IsCredentialDoc BIT,
    IsTokenGenerate BIT,
    SyncEmailAddress character varying(50)
);

-- Table: PhysicianLocation
CREATE TABLE PhysicianLocation (
    LocationId INT NOT NULL,
    PhysicianId INT NOT NULL,
    Latitude DECIMAL(9),
    Longitude DECIMAL(9),
    CreatedDate timestamp without time zone,
    PhysicianName character varying(50),
    Address character varying(500)
);

-- Table: PhysicianNotification
CREATE TABLE PhysicianNotification (
    id INT NOT NULL PRIMARY KEY,
    PhysicianId INT NOT NULL,
    IsNotificationStopped BIT
);

-- Table: PhysicianRegion
CREATE TABLE PhysicianRegion (
    PhysicianRegionId INT NOT NULL PRIMARY KEY,
    PhysicianId INT NOT NULL,
    RegionId INT NOT NULL
);

-- Table: Region
CREATE TABLE Region (
    RegionId INT NOT NULL PRIMARY KEY,
    Name character varying(50) NOT NULL,
    Abbreviation character varying(50)
);

-- Table: Request
CREATE TABLE Request (
    RequestId INT NOT NULL PRIMARY KEY,
    RequestTypeId INT NOT NULL,
    UserId INT,
    FirstName character varying(100),
    LastName character varying(100),
    PhoneNumber character varying(23),
    Email character varying(50),
    Status smallint NOT NULL,
    PhysicianId INT,
    ConfirmationNumber character varying(20),
    CreatedDate timestamp without time zone NOT NULL,
    IsDeleted BIT,
    ModifiedDate timestamp without time zone,
    DeclinedBy VARCHAR(250),
    IsUrgentEmailSent BIT,
    LastWellnessDate timestamp without time zone,
    IsMobile BIT,
    CallType smallint,
    CompletedByPhysician BIT,
    LastReservationDate timestamp without time zone,
    AcceptedDate timestamp without time zone,
    RelationName character varying(100),
    CaseNumber character varying(50),
    IP character varying(20),
    CaseTag character varying(50),
    CaseTagPhysician character varying(50),
    PatientAccountId character varying(128),
    CreatedUserId INT
);

-- Table: RequestBusiness
CREATE TABLE RequestBusiness (
    RequestBusinessId INT NOT NULL PRIMARY KEY,
    RequestId INT NOT NULL,
    BusinessId INT NOT NULL,
    IP character varying(20)
);

-- Table: RequestClient
CREATE TABLE RequestClient (
    RequestClientId INT NOT NULL PRIMARY KEY,
    RequestId INT NOT NULL,
    FirstName character varying(100) NOT NULL,
    LastName character varying(100),
    PhoneNumber character varying(23),
    Location character varying(100),
    Address character varying(500),
    RegionId INT,
    NotiMobile character varying(20),
    NotiEmail character varying(50),
    Notes character varying(500),
    Email character varying(50),
    strMonth character varying(20),
    intYear INT,
    intDate INT,
    IsMobile BIT,
    Street character varying(100),
    City character varying(100),
    State character varying(100),
    ZipCode character varying(10),
    CommunicationType smallint,
    RemindReservationCount smallint,
    RemindHouseCallCount smallint,
    IsSetFollowupSent smallint,
    IP character varying(20),
    IsReservationReminderSent smallint,
    Latitude DECIMAL(9),
    Longitude DECIMAL(9)
);

-- Table: RequestClosed
CREATE TABLE RequestClosed (
    RequestClosedId INT NOT NULL PRIMARY KEY,
    RequestId INT NOT NULL,
    RequestStatusLogId INT NOT NULL,
    PhyNotes character varying(500),
    ClientNotes character varying(500),
    IP character varying(20)
);

-- Table: RequestConcierge
CREATE TABLE RequestConcierge (
    Id INT NOT NULL PRIMARY KEY,
    RequestId INT NOT NULL,
    ConciergeId INT NOT NULL,
    IP character varying(20)
);

-- Table: RequestNotes
CREATE TABLE RequestNotes (
    RequestNotesId INT NOT NULL PRIMARY KEY,
    RequestId INT NOT NULL,
    strMonth character varying(20),
    intYear INT,
    intDate INT,
    PhysicianNotes character varying(500),
    AdminNotes character varying(500),
    CreatedBy character varying(128) NOT NULL,
    CreatedDate timestamp without time zone NOT NULL,
    ModifiedBy character varying(128),
    ModifiedDate timestamp without time zone,
    IP character varying(20),
    AdministrativeNotes character varying(500)
);

-- Table: RequestStatusLog
CREATE TABLE RequestStatusLog (
    RequestStatusLogId INT NOT NULL PRIMARY KEY,
    RequestId INT NOT NULL,
    Status smallint NOT NULL,
    PhysicianId INT,
    AdminId INT,
    TransToPhysicianId INT,
    Notes character varying(500),
    CreatedDate timestamp without time zone NOT NULL,
    IP character varying(20),
    TransToAdmin BIT
);

-- Table: RequestType
CREATE TABLE RequestType (
    RequestTypeId INT NOT NULL PRIMARY KEY,
    Name character varying(50) NOT NULL
);

-- Table: RequestWiseFile
CREATE TABLE RequestWiseFile (
    RequestWiseFileID INT NOT NULL PRIMARY KEY,
    RequestId INT NOT NULL,
    FileName character varying(500) NOT NULL,
    CreatedDate timestamp without time zone NOT NULL,
    PhysicianId INT,
    AdminId INT,
    DocType smallint,
    IsFrontSide BIT,
    IsCompensation BIT,
    IP character varying(20),
    IsFinalize BIT,
    IsDeleted BIT,
    IsPatientRecords BIT
);
-- Table: Role
CREATE TABLE Role (
    RoleId INT NOT NULL PRIMARY KEY,
    Name character varying(50) NOT NULL,
    AccountType smallint NOT NULL,
    CreatedBy character varying(128) NOT NULL,
    CreatedDate timestamp without time zone NOT NULL,
    ModifiedBy character varying(128),
    ModifiedDate timestamp without time zone,
    IsDeleted BIT NOT NULL,
    IP character varying(20)
);

-- Table: RoleMenu
CREATE TABLE RoleMenu (
    RoleMenuId INT NOT NULL PRIMARY KEY,
    RoleId INT NOT NULL,
    MenuId INT NOT NULL
);

-- Table: Shift
CREATE TABLE Shift (
    ShiftId INT NOT NULL PRIMARY KEY,
    PhysicianId INT NOT NULL,
    StartDate DATE NOT NULL,
    IsRepeat BIT NOT NULL,
    WeekDays CHAR(7),
    RepeatUpto INT,
    CreatedBy character varying(128) NOT NULL,
    CreatedDate timestamp without time zone NOT NULL,
    IP character varying(20)
);

-- Table: ShiftDetail
CREATE TABLE ShiftDetail (
    ShiftDetailId INT NOT NULL PRIMARY KEY,
    ShiftId INT NOT NULL,
    ShiftDate timestamp without time zone NOT NULL,
    RegionId INT,
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    Status smallint NOT NULL,
    IsDeleted BIT NOT NULL,
    ModifiedBy character varying(128),
    ModifiedDate timestamp without time zone,
    LastRunningDate timestamp without time zone,
    EventId character varying(100),
    IsSync BIT
);

-- Table: ShiftDetailRegion
CREATE TABLE ShiftDetailRegion (
    ShiftDetailRegionId INT NOT NULL PRIMARY KEY,
    ShiftDetailId INT NOT NULL,
    RegionId INT NOT NULL,
    IsDeleted BIT
);
-- Table: SMSLog
CREATE TABLE SMSLog (
    SMSLogID DECIMAL(9) NOT NULL PRIMARY KEY,
    SMSTemplate character varying NOT NULL,
    MobileNumber character varying(50) NOT NULL,
    ConfirmationNumber character varying(200),
    RoleId INT,
    AdminId INT,
    RequestId INT,
    PhysicianId INT,
    CreateDate timestamp without time zone NOT NULL,
    SentDate timestamp without time zone,
    IsSMSSent BIT,
    SentTries INT NOT NULL,
    Action INT
);

-- Table: User
CREATE TABLE Users (
    UserId INT NOT NULL PRIMARY KEY,
    AspNetUserId character varying(128),
    FirstName character varying(100) NOT NULL,
    LastName character varying(100),
    Email character varying(50) NOT NULL,
    Mobile character varying(20),
    IsMobile BIT,
    Street character varying(100),
    City character varying(100),
    State character varying(100),
    RegionId INT,
    ZipCode character varying(10),
    strMonth character varying(20),
    intYear INT,
    intDate INT,
    CreatedBy character varying(128) NOT NULL,
    CreatedDate timestamp without time zone NOT NULL,
    ModifiedBy character varying(128),
    ModifiedDate timestamp without time zone,
    Status smallint,
    IsDeleted BIT,
    IP character varying(20),
    IsRequestWithEmail BIT
);


