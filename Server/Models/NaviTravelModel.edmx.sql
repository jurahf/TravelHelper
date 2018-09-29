
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/26/2018 16:57:54
-- Generated from EDMX file: D:\PROJECTS\CSharp\TravelHelper\Server\Models\NaviTravelModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [TravelHelperDatabase];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserTravel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TravelSet] DROP CONSTRAINT [FK_UserTravel];
GO
IF OBJECT_ID(N'[dbo].[FK_TravelSchedule]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ScheduleSet] DROP CONSTRAINT [FK_TravelSchedule];
GO
IF OBJECT_ID(N'[dbo].[FK_TravelCategory_Travel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TravelCategory] DROP CONSTRAINT [FK_TravelCategory_Travel];
GO
IF OBJECT_ID(N'[dbo].[FK_TravelCategory_Category]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TravelCategory] DROP CONSTRAINT [FK_TravelCategory_Category];
GO
IF OBJECT_ID(N'[dbo].[FK_SchedulePlacePoint]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlacePointSet] DROP CONSTRAINT [FK_SchedulePlacePoint];
GO
IF OBJECT_ID(N'[dbo].[FK_NaviAddressInfoPlacePoint]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PlacePointSet] DROP CONSTRAINT [FK_NaviAddressInfoPlacePoint];
GO
IF OBJECT_ID(N'[dbo].[FK_CategoryNaviAddressInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NaviAddressInfoSet] DROP CONSTRAINT [FK_CategoryNaviAddressInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_CategoryCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CategorySet] DROP CONSTRAINT [FK_CategoryCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_CityTravel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TravelSet] DROP CONSTRAINT [FK_CityTravel];
GO
IF OBJECT_ID(N'[dbo].[FK_CityNaviAddressInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NaviAddressInfoSet] DROP CONSTRAINT [FK_CityNaviAddressInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_UserSettingsUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserSet] DROP CONSTRAINT [FK_UserSettingsUser];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UserSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSet];
GO
IF OBJECT_ID(N'[dbo].[TravelSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TravelSet];
GO
IF OBJECT_ID(N'[dbo].[ScheduleSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ScheduleSet];
GO
IF OBJECT_ID(N'[dbo].[PlacePointSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PlacePointSet];
GO
IF OBJECT_ID(N'[dbo].[CategorySet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CategorySet];
GO
IF OBJECT_ID(N'[dbo].[NaviAddressInfoSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NaviAddressInfoSet];
GO
IF OBJECT_ID(N'[dbo].[CitySet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CitySet];
GO
IF OBJECT_ID(N'[dbo].[UserSettingsSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSettingsSet];
GO
IF OBJECT_ID(N'[dbo].[TravelCategory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TravelCategory];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserSet'
CREATE TABLE [dbo].[UserSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Login] nvarchar(max)  NOT NULL,
    [UserSettings_Id] int  NULL
);
GO

-- Creating table 'TravelSet'
CREATE TABLE [dbo].[TravelSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [StartDate] datetime  NOT NULL,
    [EndDate] datetime  NOT NULL,
    [User_Id] int  NOT NULL,
    [City_Id] int  NOT NULL
);
GO

-- Creating table 'ScheduleSet'
CREATE TABLE [dbo].[ScheduleSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL,
    [TempPoint] int  NOT NULL,
    [Travel_Id] int  NOT NULL
);
GO

-- Creating table 'PlacePointSet'
CREATE TABLE [dbo].[PlacePointSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Order] int  NOT NULL,
    [CustomName] nvarchar(max)  NOT NULL,
    [Time] datetime  NOT NULL,
    [Schedule_Id] int  NULL,
    [NaviAddressInfo_Id] int  NOT NULL
);
GO

-- Creating table 'CategorySet'
CREATE TABLE [dbo].[CategorySet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [NaviId] nvarchar(max)  NULL,
    [Parent_Id] int  NULL
);
GO

-- Creating table 'NaviAddressInfoSet'
CREATE TABLE [dbo].[NaviAddressInfoSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContainerAddress] nvarchar(max)  NULL,
    [SelfAddress] nvarchar(max)  NULL,
    [Latitude] decimal(18,10)  NOT NULL,
    [Longitude] decimal(18,10)  NOT NULL,
    [Picture] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Category_Id] int  NULL,
    [City_Id] int  NULL
);
GO

-- Creating table 'CitySet'
CREATE TABLE [dbo].[CitySet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Country] nvarchar(max)  NULL,
    [Lat] decimal(18,10)  NOT NULL,
    [Lng] decimal(18,10)  NOT NULL
);
GO

-- Creating table 'UserSettingsSet'
CREATE TABLE [dbo].[UserSettingsSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SelectedTravelId] int  NULL
);
GO

-- Creating table 'TravelCategory'
CREATE TABLE [dbo].[TravelCategory] (
    [TravelCategory_Category_Id] int  NOT NULL,
    [Categories_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'UserSet'
ALTER TABLE [dbo].[UserSet]
ADD CONSTRAINT [PK_UserSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TravelSet'
ALTER TABLE [dbo].[TravelSet]
ADD CONSTRAINT [PK_TravelSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ScheduleSet'
ALTER TABLE [dbo].[ScheduleSet]
ADD CONSTRAINT [PK_ScheduleSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PlacePointSet'
ALTER TABLE [dbo].[PlacePointSet]
ADD CONSTRAINT [PK_PlacePointSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CategorySet'
ALTER TABLE [dbo].[CategorySet]
ADD CONSTRAINT [PK_CategorySet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NaviAddressInfoSet'
ALTER TABLE [dbo].[NaviAddressInfoSet]
ADD CONSTRAINT [PK_NaviAddressInfoSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CitySet'
ALTER TABLE [dbo].[CitySet]
ADD CONSTRAINT [PK_CitySet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserSettingsSet'
ALTER TABLE [dbo].[UserSettingsSet]
ADD CONSTRAINT [PK_UserSettingsSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [TravelCategory_Category_Id], [Categories_Id] in table 'TravelCategory'
ALTER TABLE [dbo].[TravelCategory]
ADD CONSTRAINT [PK_TravelCategory]
    PRIMARY KEY CLUSTERED ([TravelCategory_Category_Id], [Categories_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [User_Id] in table 'TravelSet'
ALTER TABLE [dbo].[TravelSet]
ADD CONSTRAINT [FK_UserTravel]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[UserSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserTravel'
CREATE INDEX [IX_FK_UserTravel]
ON [dbo].[TravelSet]
    ([User_Id]);
GO

-- Creating foreign key on [Travel_Id] in table 'ScheduleSet'
ALTER TABLE [dbo].[ScheduleSet]
ADD CONSTRAINT [FK_TravelSchedule]
    FOREIGN KEY ([Travel_Id])
    REFERENCES [dbo].[TravelSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TravelSchedule'
CREATE INDEX [IX_FK_TravelSchedule]
ON [dbo].[ScheduleSet]
    ([Travel_Id]);
GO

-- Creating foreign key on [TravelCategory_Category_Id] in table 'TravelCategory'
ALTER TABLE [dbo].[TravelCategory]
ADD CONSTRAINT [FK_TravelCategory_Travel]
    FOREIGN KEY ([TravelCategory_Category_Id])
    REFERENCES [dbo].[TravelSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Categories_Id] in table 'TravelCategory'
ALTER TABLE [dbo].[TravelCategory]
ADD CONSTRAINT [FK_TravelCategory_Category]
    FOREIGN KEY ([Categories_Id])
    REFERENCES [dbo].[CategorySet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TravelCategory_Category'
CREATE INDEX [IX_FK_TravelCategory_Category]
ON [dbo].[TravelCategory]
    ([Categories_Id]);
GO

-- Creating foreign key on [Schedule_Id] in table 'PlacePointSet'
ALTER TABLE [dbo].[PlacePointSet]
ADD CONSTRAINT [FK_SchedulePlacePoint]
    FOREIGN KEY ([Schedule_Id])
    REFERENCES [dbo].[ScheduleSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SchedulePlacePoint'
CREATE INDEX [IX_FK_SchedulePlacePoint]
ON [dbo].[PlacePointSet]
    ([Schedule_Id]);
GO

-- Creating foreign key on [NaviAddressInfo_Id] in table 'PlacePointSet'
ALTER TABLE [dbo].[PlacePointSet]
ADD CONSTRAINT [FK_NaviAddressInfoPlacePoint]
    FOREIGN KEY ([NaviAddressInfo_Id])
    REFERENCES [dbo].[NaviAddressInfoSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NaviAddressInfoPlacePoint'
CREATE INDEX [IX_FK_NaviAddressInfoPlacePoint]
ON [dbo].[PlacePointSet]
    ([NaviAddressInfo_Id]);
GO

-- Creating foreign key on [Category_Id] in table 'NaviAddressInfoSet'
ALTER TABLE [dbo].[NaviAddressInfoSet]
ADD CONSTRAINT [FK_CategoryNaviAddressInfo]
    FOREIGN KEY ([Category_Id])
    REFERENCES [dbo].[CategorySet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CategoryNaviAddressInfo'
CREATE INDEX [IX_FK_CategoryNaviAddressInfo]
ON [dbo].[NaviAddressInfoSet]
    ([Category_Id]);
GO

-- Creating foreign key on [Parent_Id] in table 'CategorySet'
ALTER TABLE [dbo].[CategorySet]
ADD CONSTRAINT [FK_CategoryCategory]
    FOREIGN KEY ([Parent_Id])
    REFERENCES [dbo].[CategorySet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CategoryCategory'
CREATE INDEX [IX_FK_CategoryCategory]
ON [dbo].[CategorySet]
    ([Parent_Id]);
GO

-- Creating foreign key on [City_Id] in table 'TravelSet'
ALTER TABLE [dbo].[TravelSet]
ADD CONSTRAINT [FK_CityTravel]
    FOREIGN KEY ([City_Id])
    REFERENCES [dbo].[CitySet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CityTravel'
CREATE INDEX [IX_FK_CityTravel]
ON [dbo].[TravelSet]
    ([City_Id]);
GO

-- Creating foreign key on [City_Id] in table 'NaviAddressInfoSet'
ALTER TABLE [dbo].[NaviAddressInfoSet]
ADD CONSTRAINT [FK_CityNaviAddressInfo]
    FOREIGN KEY ([City_Id])
    REFERENCES [dbo].[CitySet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CityNaviAddressInfo'
CREATE INDEX [IX_FK_CityNaviAddressInfo]
ON [dbo].[NaviAddressInfoSet]
    ([City_Id]);
GO

-- Creating foreign key on [UserSettings_Id] in table 'UserSet'
ALTER TABLE [dbo].[UserSet]
ADD CONSTRAINT [FK_UserSettingsUser]
    FOREIGN KEY ([UserSettings_Id])
    REFERENCES [dbo].[UserSettingsSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserSettingsUser'
CREATE INDEX [IX_FK_UserSettingsUser]
ON [dbo].[UserSet]
    ([UserSettings_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------