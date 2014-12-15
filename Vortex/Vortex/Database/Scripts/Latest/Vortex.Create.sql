
    if exists (select * from dbo.sysobjects where id = object_id(N'Users') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Users

    if exists (select * from dbo.sysobjects where id = object_id(N'NH_Hilo') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table NH_Hilo

    create table Users (
        Id BIGINT not null,
       Username NVARCHAR(190) not null,
       Email NVARCHAR(80) null,
       primary key (Id)
    )

    create table NH_Hilo (
         Users_Next_Hi BIGINT 
    )

    insert into NH_Hilo values ( 1 )
