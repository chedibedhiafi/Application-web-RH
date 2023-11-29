IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [Discriminator] nvarchar(max) NOT NULL,
    [CustomTag] nvarchar(max) NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [FonctionEmployees] (
    [FonctionEmployeeId] int NOT NULL IDENTITY,
    [Fonction] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_FonctionEmployees] PRIMARY KEY ([FonctionEmployeeId])
);
GO

CREATE TABLE [Genres] (
    [GenreId] int NOT NULL IDENTITY,
    [NomGenre] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Genres] PRIMARY KEY ([GenreId])
);
GO

CREATE TABLE [Hierarchies] (
    [HierarchieId] int NOT NULL IDENTITY,
    [Niveau] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Hierarchies] PRIMARY KEY ([HierarchieId])
);
GO

CREATE TABLE [Missions] (
    [MissionId] int NOT NULL IDENTITY,
    [Description] nvarchar(max) NOT NULL,
    [DateDebut] datetime2 NOT NULL,
    [DateFin] datetime2 NOT NULL,
    CONSTRAINT [PK_Missions] PRIMARY KEY ([MissionId])
);
GO

CREATE TABLE [Roles] (
    [RoleId] int NOT NULL IDENTITY,
    [NomRole] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([RoleId])
);
GO

CREATE TABLE [Sections] (
    [SectionId] int NOT NULL IDENTITY,
    [NomSection] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Sections] PRIMARY KEY ([SectionId])
);
GO

CREATE TABLE [Situations] (
    [SituationId] int NOT NULL IDENTITY,
    [nomSituation] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Situations] PRIMARY KEY ([SituationId])
);
GO

CREATE TABLE [typeConfirmations] (
    [TypeConfirmationId] int NOT NULL IDENTITY,
    [type] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_typeConfirmations] PRIMARY KEY ([TypeConfirmationId])
);
GO

CREATE TABLE [TypeJustificatifs] (
    [TypeJustificatifId] int NOT NULL IDENTITY,
    [type] nvarchar(max) NOT NULL,
    [Document] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_TypeJustificatifs] PRIMARY KEY ([TypeJustificatifId])
);
GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(128) NOT NULL,
    [ProviderKey] nvarchar(128) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(128) NOT NULL,
    [Name] nvarchar(128) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Employees] (
    [id] int NOT NULL IDENTITY,
    [Nom] nvarchar(max) NOT NULL,
    [NomArabe] nvarchar(max) NOT NULL,
    [Prenom] nvarchar(max) NOT NULL,
    [PrenomArabe] nvarchar(max) NOT NULL,
    [DateDeNaissance] datetime2 NOT NULL,
    [Cin] int NOT NULL,
    [CreditConges] int NOT NULL,
    [Matricule] int NOT NULL,
    [Photo] nvarchar(max) NOT NULL,
    [SectionFk] int NOT NULL,
    [StartedAt] datetime2 NOT NULL,
    [SituationFk] int NOT NULL,
    [RoleFk] int NOT NULL,
    [GenreFk] int NOT NULL,
    [FonctionEmployeeFk] int NOT NULL,
    CONSTRAINT [PK_Employees] PRIMARY KEY ([id]),
    CONSTRAINT [FK_Employees_FonctionEmployees_FonctionEmployeeFk] FOREIGN KEY ([FonctionEmployeeFk]) REFERENCES [FonctionEmployees] ([FonctionEmployeeId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Employees_Genres_GenreFk] FOREIGN KEY ([GenreFk]) REFERENCES [Genres] ([GenreId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Employees_Roles_RoleFk] FOREIGN KEY ([RoleFk]) REFERENCES [Roles] ([RoleId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Employees_Sections_SectionFk] FOREIGN KEY ([SectionFk]) REFERENCES [Sections] ([SectionId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Employees_Situations_SituationFk] FOREIGN KEY ([SituationFk]) REFERENCES [Situations] ([SituationId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Absences] (
    [AbsenceId] int NOT NULL IDENTITY,
    [nbAbsence] int NOT NULL,
    [TypeJustificatifFk] int NOT NULL,
    [EmplyeesFk] int NOT NULL,
    CONSTRAINT [PK_Absences] PRIMARY KEY ([AbsenceId]),
    CONSTRAINT [FK_Absences_Employees_EmplyeesFk] FOREIGN KEY ([EmplyeesFk]) REFERENCES [Employees] ([id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Absences_TypeJustificatifs_TypeJustificatifFk] FOREIGN KEY ([TypeJustificatifFk]) REFERENCES [TypeJustificatifs] ([TypeJustificatifId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Attestations] (
    [AttestationId] int NOT NULL IDENTITY,
    [Description] nvarchar(max) NOT NULL,
    [EmployeeFk] int NOT NULL,
    [DocumentAttestation] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Attestations] PRIMARY KEY ([AttestationId]),
    CONSTRAINT [FK_Attestations_Employees_EmployeeFk] FOREIGN KEY ([EmployeeFk]) REFERENCES [Employees] ([id]) ON DELETE CASCADE
);
GO

CREATE TABLE [ContreVisites] (
    [ContreVisiteId] int NOT NULL IDENTITY,
    [Date] datetime2 NOT NULL,
    [EffectuerPar] int NOT NULL,
    CONSTRAINT [PK_ContreVisites] PRIMARY KEY ([ContreVisiteId]),
    CONSTRAINT [FK_ContreVisites_Employees_EffectuerPar] FOREIGN KEY ([EffectuerPar]) REFERENCES [Employees] ([id]) ON DELETE CASCADE
);
GO

CREATE TABLE [GestionDocuments] (
    [GestionDocumentId] int NOT NULL IDENTITY,
    [TypeJustificatifFk] int NOT NULL,
    [AttestationFk] int NOT NULL,
    CONSTRAINT [PK_GestionDocuments] PRIMARY KEY ([GestionDocumentId]),
    CONSTRAINT [FK_GestionDocuments_Attestations_AttestationFk] FOREIGN KEY ([AttestationFk]) REFERENCES [Attestations] ([AttestationId]) ON DELETE CASCADE,
    CONSTRAINT [FK_GestionDocuments_TypeJustificatifs_TypeJustificatifFk] FOREIGN KEY ([TypeJustificatifFk]) REFERENCES [TypeJustificatifs] ([TypeJustificatifId]) ON DELETE CASCADE
);
GO

CREATE TABLE [TypeConges] (
    [TypeCongesId] int NOT NULL IDENTITY,
    [Designation] nvarchar(max) NOT NULL,
    [nbMaximumdeJour] int NOT NULL,
    [NecessiteCv] bit NOT NULL,
    [ContreVisiteId] int NOT NULL,
    CONSTRAINT [PK_TypeConges] PRIMARY KEY ([TypeCongesId]),
    CONSTRAINT [FK_TypeConges_ContreVisites_ContreVisiteId] FOREIGN KEY ([ContreVisiteId]) REFERENCES [ContreVisites] ([ContreVisiteId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Conges] (
    [CongesId] int NOT NULL IDENTITY,
    [DateDebut] datetime2 NOT NULL,
    [DateFin] datetime2 NOT NULL,
    [Raison] nvarchar(max) NOT NULL,
    [IdEmployees] int NOT NULL,
    [ConfirmePar] int NOT NULL,
    [RemplassePar] int NOT NULL,
    [TypeCongesFk] int NOT NULL,
    [TypeConfirmationFk] int NOT NULL,
    CONSTRAINT [PK_Conges] PRIMARY KEY ([CongesId]),
    CONSTRAINT [FK_Conges_Employees_ConfirmePar] FOREIGN KEY ([ConfirmePar]) REFERENCES [Employees] ([id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Conges_Employees_IdEmployees] FOREIGN KEY ([IdEmployees]) REFERENCES [Employees] ([id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Conges_Employees_RemplassePar] FOREIGN KEY ([RemplassePar]) REFERENCES [Employees] ([id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Conges_typeConfirmations_TypeConfirmationFk] FOREIGN KEY ([TypeConfirmationFk]) REFERENCES [typeConfirmations] ([TypeConfirmationId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Conges_TypeConges_TypeCongesFk] FOREIGN KEY ([TypeCongesFk]) REFERENCES [TypeConges] ([TypeCongesId]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Absences_EmplyeesFk] ON [Absences] ([EmplyeesFk]);
GO

CREATE INDEX [IX_Absences_TypeJustificatifFk] ON [Absences] ([TypeJustificatifFk]);
GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

CREATE INDEX [IX_Attestations_EmployeeFk] ON [Attestations] ([EmployeeFk]);
GO

CREATE INDEX [IX_Conges_ConfirmePar] ON [Conges] ([ConfirmePar]);
GO

CREATE INDEX [IX_Conges_IdEmployees] ON [Conges] ([IdEmployees]);
GO

CREATE INDEX [IX_Conges_RemplassePar] ON [Conges] ([RemplassePar]);
GO

CREATE INDEX [IX_Conges_TypeConfirmationFk] ON [Conges] ([TypeConfirmationFk]);
GO

CREATE INDEX [IX_Conges_TypeCongesFk] ON [Conges] ([TypeCongesFk]);
GO

CREATE INDEX [IX_ContreVisites_EffectuerPar] ON [ContreVisites] ([EffectuerPar]);
GO

CREATE INDEX [IX_Employees_FonctionEmployeeFk] ON [Employees] ([FonctionEmployeeFk]);
GO

CREATE INDEX [IX_Employees_GenreFk] ON [Employees] ([GenreFk]);
GO

CREATE INDEX [IX_Employees_RoleFk] ON [Employees] ([RoleFk]);
GO

CREATE INDEX [IX_Employees_SectionFk] ON [Employees] ([SectionFk]);
GO

CREATE INDEX [IX_Employees_SituationFk] ON [Employees] ([SituationFk]);
GO

CREATE INDEX [IX_GestionDocuments_AttestationFk] ON [GestionDocuments] ([AttestationFk]);
GO

CREATE INDEX [IX_GestionDocuments_TypeJustificatifFk] ON [GestionDocuments] ([TypeJustificatifFk]);
GO

CREATE INDEX [IX_TypeConges_ContreVisiteId] ON [TypeConges] ([ContreVisiteId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230824210106_ApplicationUser', N'6.0.21');
GO

COMMIT;
GO

