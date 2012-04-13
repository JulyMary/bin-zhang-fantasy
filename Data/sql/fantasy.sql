/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     13/04/2012 4:40:08 PM                        */
/*==============================================================*/


if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('AssemblyReference') and o.name = 'FK_ASSEMBLYGROUP_ASSEMBLY')
alter table AssemblyReference
   drop constraint FK_ASSEMBLYGROUP_ASSEMBLY
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BusinessApplication') and o.name = 'FK_BUSINESS_APPLICATION_ENTRYOBJECT')
alter table BusinessApplication
   drop constraint FK_BUSINESS_APPLICATION_ENTRYOBJECT
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BusinessApplication') and o.name = 'FK_BUSINESS_PACKAGE_APPLICATION')
alter table BusinessApplication
   drop constraint FK_BUSINESS_PACKAGE_APPLICATION
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BusinessApplicationACL') and o.name = 'FK_BUSINESS_PARTICIPANT_ACL')
alter table BusinessApplicationACL
   drop constraint FK_BUSINESS_PARTICIPANT_ACL
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BusinessApplicationACL') and o.name = 'FK_BUSINESSAPPACL_STATE')
alter table BusinessApplicationACL
   drop constraint FK_BUSINESSAPPACL_STATE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BusinessApplicationACL') and o.name = 'FK_BUSINESS_ROLE_ACL')
alter table BusinessApplicationACL
   drop constraint FK_BUSINESS_ROLE_ACL
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BusinessApplicationParticipant') and o.name = 'FK_BUSINESS_PARTICIPANT_CLASS')
alter table BusinessApplicationParticipant
   drop constraint FK_BUSINESS_PARTICIPANT_CLASS
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BusinessApplicationParticipant') and o.name = 'FK_BUSINESS_APPLICATION_PARTICIPANT')
alter table BusinessApplicationParticipant
   drop constraint FK_BUSINESS_APPLICATION_PARTICIPANT
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BusinessAssociation') and o.name = 'FK_BUSINESS_PACKAGEA_ASSOCIATION')
alter table BusinessAssociation
   drop constraint FK_BUSINESS_PACKAGEA_ASSOCIATION
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BusinessClass') and o.name = 'FK_BUSINESS_PACKAGE_CLASS')
alter table BusinessClass
   drop constraint FK_BUSINESS_PACKAGE_CLASS
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BusinessClass') and o.name = 'FK_BUSINESS_CHILDCLASS')
alter table BusinessClass
   drop constraint FK_BUSINESS_CHILDCLASS
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BusinessClassDiagram') and o.name = 'FK_BUSINESS_PACKAGE_CLASSDIAGRAM')
alter table BusinessClassDiagram
   drop constraint FK_BUSINESS_PACKAGE_CLASSDIAGRAM
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BusinessEnum') and o.name = 'FK_BUSINESS_PACKAGE_ENUM')
alter table BusinessEnum
   drop constraint FK_BUSINESS_PACKAGE_ENUM
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BusinessEnumValue') and o.name = 'FK_BUSINESS_ENUM_VALUE')
alter table BusinessEnumValue
   drop constraint FK_BUSINESS_ENUM_VALUE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BusinessExtraScript') and o.name = 'FK_BUSINESS_PACKAGE_SCRIPT')
alter table BusinessExtraScript
   drop constraint FK_BUSINESS_PACKAGE_SCRIPT
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BusinessObject') and o.name = 'FK_BUSINESS_CLASS_OBJECT')
alter table BusinessObject
   drop constraint FK_BUSINESS_CLASS_OBJECT
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BusinessPackage') and o.name = 'FK_BUSINESS_CHILDPACKAGES')
alter table BusinessPackage
   drop constraint FK_BUSINESS_CHILDPACKAGES
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BusinessProperty') and o.name = 'FK_BUSINESS_CLASS_PROPERTY')
alter table BusinessProperty
   drop constraint FK_BUSINESS_CLASS_PROPERTY
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BusinessProperty') and o.name = 'FK_BUSINESS_PROPTYPE_CLASS')
alter table BusinessProperty
   drop constraint FK_BUSINESS_PROPTYPE_CLASS
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BusinessProperty') and o.name = 'FK_BUSINESS_PROPTYPE_ENUM')
alter table BusinessProperty
   drop constraint FK_BUSINESS_PROPTYPE_ENUM
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BusinessProperty') and o.name = 'FK_BUSINESS_DATATYPE_PROPERTY')
alter table BusinessProperty
   drop constraint FK_BUSINESS_DATATYPE_PROPERTY
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BusinessRole') and o.name = 'FK_BUSINESS_PACKAGE_ROLE')
alter table BusinessRole
   drop constraint FK_BUSINESS_PACKAGE_ROLE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BusinessUser') and o.name = 'FK_BUSINESS_PACKAGE_USER')
alter table BusinessUser
   drop constraint FK_BUSINESS_PACKAGE_USER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BusinessUserRole') and o.name = 'FK_BUSINESS_USER_ROLES')
alter table BusinessUserRole
   drop constraint FK_BUSINESS_USER_ROLES
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BusinessUserRole') and o.name = 'FK_BUSINESS_ROLE_USERS')
alter table BusinessUserRole
   drop constraint FK_BUSINESS_ROLE_USERS
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('AssemblyReference')
            and   name  = 'ASSEMBLYGROUPREFERENCE_FK'
            and   indid > 0
            and   indid < 255)
   drop index AssemblyReference.ASSEMBLYGROUPREFERENCE_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('AssemblyReference')
            and   type = 'U')
   drop table AssemblyReference
go

if exists (select 1
            from  sysobjects
           where  id = object_id('AssemblyReferenceGroup')
            and   type = 'U')
   drop table AssemblyReferenceGroup
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BusinessApplication')
            and   name  = 'BUSINESSPACKAGEAPPLICATIONS_FK'
            and   indid > 0
            and   indid < 255)
   drop index BusinessApplication.BUSINESSPACKAGEAPPLICATIONS_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BusinessApplication')
            and   name  = 'BUSINESSAPPLICATIONENTRYOBJECT_FK'
            and   indid > 0
            and   indid < 255)
   drop index BusinessApplication.BUSINESSAPPLICATIONENTRYOBJECT_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BusinessApplication')
            and   type = 'U')
   drop table BusinessApplication
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BusinessApplicationACL')
            and   name  = 'APPLICATIONPARTICIPANTACL_FK'
            and   indid > 0
            and   indid < 255)
   drop index BusinessApplicationACL.APPLICATIONPARTICIPANTACL_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BusinessApplicationACL')
            and   name  = 'BUSINESSAPPLICATIONACLSTATE_FK'
            and   indid > 0
            and   indid < 255)
   drop index BusinessApplicationACL.BUSINESSAPPLICATIONACLSTATE_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BusinessApplicationACL')
            and   name  = 'CLASSACCESSCONTROLROLE_FK'
            and   indid > 0
            and   indid < 255)
   drop index BusinessApplicationACL.CLASSACCESSCONTROLROLE_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BusinessApplicationACL')
            and   type = 'U')
   drop table BusinessApplicationACL
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BusinessApplicationParticipant')
            and   name  = 'PARTICIPANTCLASS_FK'
            and   indid > 0
            and   indid < 255)
   drop index BusinessApplicationParticipant.PARTICIPANTCLASS_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BusinessApplicationParticipant')
            and   name  = 'PATICIPANTAPPLICATION_FK'
            and   indid > 0
            and   indid < 255)
   drop index BusinessApplicationParticipant.PATICIPANTAPPLICATION_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BusinessApplicationParticipant')
            and   type = 'U')
   drop table BusinessApplicationParticipant
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BusinessAssociation')
            and   name  = 'PACKAGEASSOCIATIONS_FK'
            and   indid > 0
            and   indid < 255)
   drop index BusinessAssociation.PACKAGEASSOCIATIONS_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BusinessAssociation')
            and   name  = 'RELATIONSHIPRIGHT_FK'
            and   indid > 0
            and   indid < 255)
   drop index BusinessAssociation.RELATIONSHIPRIGHT_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BusinessAssociation')
            and   name  = 'RELATIONSHIPLEFT_FK'
            and   indid > 0
            and   indid < 255)
   drop index BusinessAssociation.RELATIONSHIPLEFT_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BusinessAssociation')
            and   type = 'U')
   drop table BusinessAssociation
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BusinessClass')
            and   name  = 'PACKAGECLASSES_FK'
            and   indid > 0
            and   indid < 255)
   drop index BusinessClass.PACKAGECLASSES_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BusinessClass')
            and   name  = 'PARENTCLASS_FK'
            and   indid > 0
            and   indid < 255)
   drop index BusinessClass.PARENTCLASS_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BusinessClass')
            and   type = 'U')
   drop table BusinessClass
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BusinessClassDiagram')
            and   name  = 'PACKAGECLASSDIAGRAMS_FK'
            and   indid > 0
            and   indid < 255)
   drop index BusinessClassDiagram.PACKAGECLASSDIAGRAMS_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BusinessClassDiagram')
            and   type = 'U')
   drop table BusinessClassDiagram
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BusinessDataType')
            and   type = 'U')
   drop table BusinessDataType
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BusinessEnum')
            and   name  = 'PACKAGEENUM_FK'
            and   indid > 0
            and   indid < 255)
   drop index BusinessEnum.PACKAGEENUM_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BusinessEnum')
            and   type = 'U')
   drop table BusinessEnum
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BusinessEnumValue')
            and   name  = 'ENUMVALUES_FK'
            and   indid > 0
            and   indid < 255)
   drop index BusinessEnumValue.ENUMVALUES_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BusinessEnumValue')
            and   type = 'U')
   drop table BusinessEnumValue
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BusinessExtraScript')
            and   name  = 'PACKAGEEXTRASCRIPTS_FK'
            and   indid > 0
            and   indid < 255)
   drop index BusinessExtraScript.PACKAGEEXTRASCRIPTS_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BusinessExtraScript')
            and   type = 'U')
   drop table BusinessExtraScript
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BusinessLastUpdateTimestamp')
            and   type = 'U')
   drop table BusinessLastUpdateTimestamp
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BusinessObject')
            and   name  = 'BUSINESSCLASSOBJECT_FK'
            and   indid > 0
            and   indid < 255)
   drop index BusinessObject.BUSINESSCLASSOBJECT_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BusinessObject')
            and   type = 'U')
   drop table BusinessObject
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BusinessPackage')
            and   name  = 'CHILDPACKAGES_FK'
            and   indid > 0
            and   indid < 255)
   drop index BusinessPackage.CHILDPACKAGES_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BusinessPackage')
            and   type = 'U')
   drop table BusinessPackage
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BusinessProperty')
            and   name  = 'PROPERTYCLASSTYPE_FK'
            and   indid > 0
            and   indid < 255)
   drop index BusinessProperty.PROPERTYCLASSTYPE_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BusinessProperty')
            and   name  = 'PROPERTYTYPE_FK'
            and   indid > 0
            and   indid < 255)
   drop index BusinessProperty.PROPERTYTYPE_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BusinessProperty')
            and   name  = 'PROPERTYENUMTYPE_FK'
            and   indid > 0
            and   indid < 255)
   drop index BusinessProperty.PROPERTYENUMTYPE_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BusinessProperty')
            and   name  = 'PROPERTIES_FK'
            and   indid > 0
            and   indid < 255)
   drop index BusinessProperty.PROPERTIES_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BusinessProperty')
            and   type = 'U')
   drop table BusinessProperty
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BusinessRole')
            and   name  = 'BUSINESSPACKAGEROLES_FK'
            and   indid > 0
            and   indid < 255)
   drop index BusinessRole.BUSINESSPACKAGEROLES_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BusinessRole')
            and   type = 'U')
   drop table BusinessRole
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BusinessUser')
            and   name  = 'BUSINESSPACKAGEUSERS_FK'
            and   indid > 0
            and   indid < 255)
   drop index BusinessUser.BUSINESSPACKAGEUSERS_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BusinessUser')
            and   type = 'U')
   drop table BusinessUser
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BusinessUserRole')
            and   name  = 'USERS_FK'
            and   indid > 0
            and   indid < 255)
   drop index BusinessUserRole.USERS_FK
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('BusinessUserRole')
            and   name  = 'ROLES_FK'
            and   indid > 0
            and   indid < 255)
   drop index BusinessUserRole.ROLES_FK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BusinessUserRole')
            and   type = 'U')
   drop table BusinessUserRole
go

if exists(select 1 from systypes where name='T_DBEntity')
   drop type T_DBEntity
go

if exists(select 1 from systypes where name='T_Guid')
   drop type T_Guid
go

if exists(select 1 from systypes where name='T_Name')
   drop type T_Name
go

/*==============================================================*/
/* Domain: T_DBEntity                                           */
/*==============================================================*/
create type T_DBEntity
   from varchar(64)
go

/*==============================================================*/
/* Domain: T_Guid                                               */
/*==============================================================*/
create type T_Guid
   from uniqueidentifier
go

/*==============================================================*/
/* Domain: T_Name                                               */
/*==============================================================*/
create type T_Name
   from varchar(64)
go

/*==============================================================*/
/* Table: AssemblyReference                                     */
/*==============================================================*/
create table AssemblyReference (
   ID                   T_Guid               not null,
   GROUPID              T_Guid               not null,
   ModificationTime     datetime             not null,
   CreationTime         datetime             not null,
   IsSystem             bit                  not null,
   FullName             varchar(255)         not null,
   Source               int                  not null,
   RawAssembly          varbinary(Max)       null,
   RawHash              varchar(32)          null,
   constraint PK_ASSEMBLYREFERENCE primary key (ID)
)
go

/*==============================================================*/
/* Index: ASSEMBLYGROUPREFERENCE_FK                             */
/*==============================================================*/
create index ASSEMBLYGROUPREFERENCE_FK on AssemblyReference (
GROUPID ASC
)
go

/*==============================================================*/
/* Table: AssemblyReferenceGroup                                */
/*==============================================================*/
create table AssemblyReferenceGroup (
   ID                   T_Guid               not null,
   ModificationTime     datetime             not null,
   CreationTime         datetime             not null,
   IsSystem             bit                  not null,
   constraint PK_ASSEMBLYREFERENCEGROUP primary key (ID)
)
go

/*==============================================================*/
/* Table: BusinessApplication                                   */
/*==============================================================*/
create table BusinessApplication (
   ID                   T_Guid               not null,
   ModificationTime     datetime             not null,
   CreationTime         datetime             not null,
   IsSystem             bit                  not null,
   PACKAGEID            T_Guid               not null,
   EntryObjectId        T_Guid               null,
   Name                 T_Name               not null,
   CodeName             T_Name               not null,
   Script               varchar(max)         null,
   ScriptOptions        int                  null,
   ExternalType         varchar(1024)        null,
   ViewSettings         varchar(max)         null,
   constraint PK_BUSINESSAPPLICATION primary key (ID)
)
go

/*==============================================================*/
/* Index: BUSINESSAPPLICATIONENTRYOBJECT_FK                     */
/*==============================================================*/
create index BUSINESSAPPLICATIONENTRYOBJECT_FK on BusinessApplication (
EntryObjectId ASC
)
go

/*==============================================================*/
/* Index: BUSINESSPACKAGEAPPLICATIONS_FK                        */
/*==============================================================*/
create index BUSINESSPACKAGEAPPLICATIONS_FK on BusinessApplication (
PACKAGEID ASC
)
go

/*==============================================================*/
/* Table: BusinessApplicationACL                                */
/*==============================================================*/
create table BusinessApplicationACL (
   ID                   T_Guid               not null,
   STATEID              T_Guid               null,
   PARTICIPANTID        T_Guid               not null,
   ROLEID               T_Guid               not null,
   ModificationTime     datetime             not null,
   CreationTime         datetime             not null,
   IsSystem             bit                  not null,
   ACL                  text                 null,
   constraint PK_BUSINESSAPPLICATIONACL primary key (ID)
)
go

/*==============================================================*/
/* Index: CLASSACCESSCONTROLROLE_FK                             */
/*==============================================================*/
create index CLASSACCESSCONTROLROLE_FK on BusinessApplicationACL (
ROLEID ASC
)
go

/*==============================================================*/
/* Index: BUSINESSAPPLICATIONACLSTATE_FK                        */
/*==============================================================*/
create index BUSINESSAPPLICATIONACLSTATE_FK on BusinessApplicationACL (
STATEID ASC
)
go

/*==============================================================*/
/* Index: APPLICATIONPARTICIPANTACL_FK                          */
/*==============================================================*/
create index APPLICATIONPARTICIPANTACL_FK on BusinessApplicationACL (
PARTICIPANTID ASC
)
go

/*==============================================================*/
/* Table: BusinessApplicationParticipant                        */
/*==============================================================*/
create table BusinessApplicationParticipant (
   ID                   T_Guid               not null,
   CLASSID              T_Guid               not null,
   APPLICATIONID        T_Guid               not null,
   ModificationTime     datetime             not null,
   CreationTime         datetime             not null,
   IsSystem             bit                  not null,
   IsEntry              bit                  not null,
   constraint PK_BUSINESSAPPLICATIONPARTICIP primary key (ID)
)
go

/*==============================================================*/
/* Index: PATICIPANTAPPLICATION_FK                              */
/*==============================================================*/
create index PATICIPANTAPPLICATION_FK on BusinessApplicationParticipant (
APPLICATIONID ASC
)
go

/*==============================================================*/
/* Index: PARTICIPANTCLASS_FK                                   */
/*==============================================================*/
create index PARTICIPANTCLASS_FK on BusinessApplicationParticipant (
CLASSID ASC
)
go

/*==============================================================*/
/* Table: BusinessAssociation                                   */
/*==============================================================*/
create table BusinessAssociation (
   ID                   T_Guid               not null,
   PackageID            T_Guid               not null,
   LEFTCLASSID          T_Guid               not null,
   ModificationTime     datetime             not null,
   CreationTime         datetime             not null,
   IsSystem             bit                  not null,
   RightClassID         T_Guid               not null,
   Name                 T_Name               not null,
   CodeName             T_Name               not null,
   TableName            T_DBEntity           not null,
   TableSchema          T_DBEntity           not null,
   TableSpace           T_DBEntity           null,
   LeftRoleName         T_Name               null,
   LeftRoleCode         T_Name               null,
   LeftCardinality      varchar(16)          not null,
   LeftNavigatable      bit                  not null,
   LeftRoleDisplayOrder bigint               null,
   RightRoleName        T_Name               null,
   RightRoleCode        T_Name               null,
   RightCardinality     varchar(16)          not null,
   RightNavigatable     bit                  not null,
   RightRoleDisplayOrder bigint               null,
   constraint PK_BUSINESSASSOCIATION primary key (ID)
)
go

/*==============================================================*/
/* Index: RELATIONSHIPLEFT_FK                                   */
/*==============================================================*/
create index RELATIONSHIPLEFT_FK on BusinessAssociation (
LEFTCLASSID ASC
)
go

/*==============================================================*/
/* Index: RELATIONSHIPRIGHT_FK                                  */
/*==============================================================*/
create index RELATIONSHIPRIGHT_FK on BusinessAssociation (
RightClassID ASC
)
go

/*==============================================================*/
/* Index: PACKAGEASSOCIATIONS_FK                                */
/*==============================================================*/
create index PACKAGEASSOCIATIONS_FK on BusinessAssociation (
PackageID ASC
)
go

/*==============================================================*/
/* Table: BusinessClass                                         */
/*==============================================================*/
create table BusinessClass (
   ID                   T_Guid               not null,
   ModificationTime     datetime             not null,
   CreationTime         datetime             not null,
   IsSystem             bit                  not null,
   PackageID            T_Guid               not null,
   ParentClassID        T_Guid               null,
   Name                 T_Name               not null,
   CodeName             T_Name               not null,
   TableName            T_DBEntity           not null,
   TableSchema          T_DBEntity           not null,
   TableSpace           T_DBEntity           null,
   IsSimple             bit                  not null,
   Implements           varchar(4096)        null,
   AutoScript           varchar(max)         null,
   Script               varchar(max)         null,
   ScriptOptions        int                  null,
   IsAbstract           bit                  not null,
   ExternalType         varchar(1024)        null,
   constraint PK_BUSINESSCLASS primary key (ID)
)
go

/*==============================================================*/
/* Index: PARENTCLASS_FK                                        */
/*==============================================================*/
create index PARENTCLASS_FK on BusinessClass (
ParentClassID ASC
)
go

/*==============================================================*/
/* Index: PACKAGECLASSES_FK                                     */
/*==============================================================*/
create index PACKAGECLASSES_FK on BusinessClass (
PackageID ASC
)
go

/*==============================================================*/
/* Table: BusinessClassDiagram                                  */
/*==============================================================*/
create table BusinessClassDiagram (
   ID                   T_Guid               not null,
   PACKAGEID            T_Guid               not null,
   ModificationTime     datetime             not null,
   CreationTime         datetime             not null,
   IsSystem             bit                  not null,
   Name                 T_Name               not null,
   Diagram              text                 null,
   constraint PK_BUSINESSCLASSDIAGRAM primary key (ID, PACKAGEID)
)
go

/*==============================================================*/
/* Index: PACKAGECLASSDIAGRAMS_FK                               */
/*==============================================================*/
create index PACKAGECLASSDIAGRAMS_FK on BusinessClassDiagram (
PACKAGEID ASC
)
go

/*==============================================================*/
/* Table: BusinessDataType                                      */
/*==============================================================*/
create table BusinessDataType (
   ID                   T_Guid               not null,
   DefaultDBType        T_Name               null,
   DefaultLength        int                  null,
   DefaultPrecision     int                  null,
   NHType               T_Name               null,
   Name                 T_Name               not null,
   CodeName             T_Name               not null,
   ModificationTime     datetime             not null,
   CreationTime         datetime             not null,
   IsSystem             bit                  not null,
   constraint PK_BUSINESSDATATYPE primary key (ID)
)
go

/*==============================================================*/
/* Table: BusinessEnum                                          */
/*==============================================================*/
create table BusinessEnum (
   ID                   T_Guid               not null,
   ModificationTime     datetime             not null,
   CreationTime         datetime             not null,
   IsSystem             bit                  not null,
   PACKAGEID            T_Guid               not null,
   Name                 T_Name               not null,
   CodeName             T_Name               not null,
   IsFlags              bit                  not null,
   IsExternal           bit                  null,
   ExternalAssembly     T_Name               null,
   ExternalNamespace    varchar(255)         null,
   constraint PK_BUSINESSENUM primary key (ID)
)
go

/*==============================================================*/
/* Index: PACKAGEENUM_FK                                        */
/*==============================================================*/
create index PACKAGEENUM_FK on BusinessEnum (
PACKAGEID ASC
)
go

/*==============================================================*/
/* Table: BusinessEnumValue                                     */
/*==============================================================*/
create table BusinessEnumValue (
   ID                   T_Guid               not null,
   ModificationTime     datetime             not null,
   CreationTime         datetime             not null,
   IsSystem             bit                  not null,
   ENUMID               T_Guid               not null,
   Name                 T_Name               not null,
   CodeName             T_Name               not null,
   Value                int                  not null,
   constraint PK_BUSINESSENUMVALUE primary key (ID)
)
go

/*==============================================================*/
/* Index: ENUMVALUES_FK                                         */
/*==============================================================*/
create index ENUMVALUES_FK on BusinessEnumValue (
ENUMID ASC
)
go

/*==============================================================*/
/* Table: BusinessExtraScript                                   */
/*==============================================================*/
create table BusinessExtraScript (
   ID                   T_Guid               not null,
   PackageID            T_Guid               not null,
   ModificationTime     datetime             not null,
   CreationTime         datetime             not null,
   IsSystem             bit                  not null,
   Name                 T_Name               not null,
   Script               varchar(max)         null,
   MetaData             varchar(1024)        null,
   constraint PK_BUSINESSEXTRASCRIPT primary key (ID)
)
go

/*==============================================================*/
/* Index: PACKAGEEXTRASCRIPTS_FK                                */
/*==============================================================*/
create index PACKAGEEXTRASCRIPTS_FK on BusinessExtraScript (
PackageID ASC
)
go

/*==============================================================*/
/* Table: BusinessLastUpdateTimestamp                           */
/*==============================================================*/
create table BusinessLastUpdateTimestamp (
   Name                 T_Name               null,
   Seconds              bigint               null
)
go

/*==============================================================*/
/* Table: BusinessObject                                        */
/*==============================================================*/
create table BusinessObject (
   ID                   T_Guid               not null,
   CLASSID              T_Guid               not null,
   ModificationTime     datetime             not null,
   CreationTime         datetime             not null,
   IsSystem             bit                  not null,
   Name                 T_Name               null,
   constraint PK_BUSINESSOBJECT primary key (ID)
)
go

/*==============================================================*/
/* Index: BUSINESSCLASSOBJECT_FK                                */
/*==============================================================*/
create index BUSINESSCLASSOBJECT_FK on BusinessObject (
CLASSID ASC
)
go

/*==============================================================*/
/* Table: BusinessPackage                                       */
/*==============================================================*/
create table BusinessPackage (
   ID                   T_Guid               not null,
   ParentPackageID      T_Guid               null,
   PackageType          int                  null,
   Name                 T_Name               not null,
   CodeName             T_Name               not null,
   ModificationTime     datetime             not null,
   CreationTime         datetime             not null,
   IsSystem             bit                  not null,
   constraint PK_BUSINESSPACKAGE primary key (ID)
)
go

/*==============================================================*/
/* Index: CHILDPACKAGES_FK                                      */
/*==============================================================*/
create index CHILDPACKAGES_FK on BusinessPackage (
ParentPackageID ASC
)
go

/*==============================================================*/
/* Table: BusinessProperty                                      */
/*==============================================================*/
create table BusinessProperty (
   ID                   T_Guid               not null,
   CLASSID              T_Guid               not null,
   ModificationTime     datetime             not null,
   CreationTime         datetime             not null,
   IsSystem             bit                  not null,
   ClassTypeID          T_Guid               null,
   EnumTypeID           T_Guid               null,
   DataTypeID           T_Guid               null,
   Name                 T_Name               not null,
   CodeName             T_Name               not null,
   FieldName            T_DBEntity           null,
   FieldType            T_DBEntity           null,
   Length               int                  null,
   "Precision"          int                  null,
   IsCalculated         bit                  not null,
   IsNullable           bit                  not null,
   DefaultValue         varchar(64)          null,
   ScriptOptions        int                  null,
   DisplayOrder         bigint               not null,
   constraint PK_BUSINESSPROPERTY primary key (ID, CLASSID)
)
go

/*==============================================================*/
/* Index: PROPERTIES_FK                                         */
/*==============================================================*/
create index PROPERTIES_FK on BusinessProperty (
CLASSID ASC
)
go

/*==============================================================*/
/* Index: PROPERTYENUMTYPE_FK                                   */
/*==============================================================*/
create index PROPERTYENUMTYPE_FK on BusinessProperty (
EnumTypeID ASC
)
go

/*==============================================================*/
/* Index: PROPERTYTYPE_FK                                       */
/*==============================================================*/
create index PROPERTYTYPE_FK on BusinessProperty (
DataTypeID ASC
)
go

/*==============================================================*/
/* Index: PROPERTYCLASSTYPE_FK                                  */
/*==============================================================*/
create index PROPERTYCLASSTYPE_FK on BusinessProperty (
ClassTypeID ASC
)
go

/*==============================================================*/
/* Table: BusinessRole                                          */
/*==============================================================*/
create table BusinessRole (
   ID                   T_Guid               not null,
   ModificationTime     datetime             not null,
   CreationTime         datetime             not null,
   IsSystem             bit                  not null,
   PACKAGEID            T_Guid               not null,
   Name                 T_Name               not null,
   CodeName             T_Name               not null,
   Description          varchar(1024)        null,
   IsComputed           bit                  not null,
   IsDisabled           bit                  null,
   Script               varchar(max)         null,
   ScriptOptions        int                  null,
   ExternalType         varchar(1024)        null,
   constraint PK_BUSINESSROLE primary key (ID)
)
go

/*==============================================================*/
/* Index: BUSINESSPACKAGEROLES_FK                               */
/*==============================================================*/
create index BUSINESSPACKAGEROLES_FK on BusinessRole (
PACKAGEID ASC
)
go

/*==============================================================*/
/* Table: BusinessUser                                          */
/*==============================================================*/
create table BusinessUser (
   ID                   T_Guid               not null,
   ModificationTime     datetime             not null,
   CreationTime         datetime             not null,
   IsSystem             bit                  not null,
   PACKAGEID            T_Guid               not null,
   Name                 T_Name               not null,
   CodeName             T_Name               not null,
   FullName             T_Name               not null,
   Description          varchar(1024)        null,
   Password             varchar(128)         null,
   IsDisabled           bit                  not null,
   Script               varchar(max)         null,
   ScriptOptions        int                  null,
   ExternalType         varchar(1024)        null,
   constraint PK_BUSINESSUSER primary key (ID)
)
go

/*==============================================================*/
/* Index: BUSINESSPACKAGEUSERS_FK                               */
/*==============================================================*/
create index BUSINESSPACKAGEUSERS_FK on BusinessUser (
PACKAGEID ASC
)
go

/*==============================================================*/
/* Table: BusinessUserRole                                      */
/*==============================================================*/
create table BusinessUserRole (
   USERID               T_Guid               not null,
   ROLEID               T_Guid               not null,
   constraint PK_BUSINESSUSERROLE primary key (USERID, ROLEID)
)
go

/*==============================================================*/
/* Index: ROLES_FK                                              */
/*==============================================================*/
create index ROLES_FK on BusinessUserRole (
USERID ASC
)
go

/*==============================================================*/
/* Index: USERS_FK                                              */
/*==============================================================*/
create index USERS_FK on BusinessUserRole (
ROLEID ASC
)
go

alter table AssemblyReference
   add constraint FK_ASSEMBLYGROUP_ASSEMBLY foreign key (GROUPID)
      references AssemblyReferenceGroup (ID)
         on delete cascade
go

alter table BusinessApplication
   add constraint FK_BUSINESS_APPLICATION_ENTRYOBJECT foreign key (EntryObjectId)
      references BusinessObject (ID)
go

alter table BusinessApplication
   add constraint FK_BUSINESS_PACKAGE_APPLICATION foreign key (PACKAGEID)
      references BusinessPackage (ID)
go

alter table BusinessApplicationACL
   add constraint FK_BUSINESS_PARTICIPANT_ACL foreign key (PARTICIPANTID)
      references BusinessApplicationParticipant (ID)
         on delete cascade
go

alter table BusinessApplicationACL
   add constraint FK_BUSINESSAPPACL_STATE foreign key (STATEID)
      references BusinessEnumValue (ID)
go

alter table BusinessApplicationACL
   add constraint FK_BUSINESS_ROLE_ACL foreign key (ROLEID)
      references BusinessRole (ID)
         on delete cascade
go

alter table BusinessApplicationParticipant
   add constraint FK_BUSINESS_PARTICIPANT_CLASS foreign key (CLASSID)
      references BusinessClass (ID)
go

alter table BusinessApplicationParticipant
   add constraint FK_BUSINESS_APPLICATION_PARTICIPANT foreign key (APPLICATIONID)
      references BusinessApplication (ID)
go

alter table BusinessAssociation
   add constraint FK_BUSINESS_PACKAGEA_ASSOCIATION foreign key (PackageID)
      references BusinessPackage (ID)
go

alter table BusinessClass
   add constraint FK_BUSINESS_PACKAGE_CLASS foreign key (PackageID)
      references BusinessPackage (ID)
go

alter table BusinessClass
   add constraint FK_BUSINESS_CHILDCLASS foreign key (ParentClassID)
      references BusinessClass (ID)
go

alter table BusinessClassDiagram
   add constraint FK_BUSINESS_PACKAGE_CLASSDIAGRAM foreign key (PACKAGEID)
      references BusinessPackage (ID)
go

alter table BusinessEnum
   add constraint FK_BUSINESS_PACKAGE_ENUM foreign key (PACKAGEID)
      references BusinessPackage (ID)
go

alter table BusinessEnumValue
   add constraint FK_BUSINESS_ENUM_VALUE foreign key (ENUMID)
      references BusinessEnum (ID)
         on delete cascade
go

alter table BusinessExtraScript
   add constraint FK_BUSINESS_PACKAGE_SCRIPT foreign key (PackageID)
      references BusinessPackage (ID)
go

alter table BusinessObject
   add constraint FK_BUSINESS_CLASS_OBJECT foreign key (CLASSID)
      references BusinessClass (ID)
go

alter table BusinessPackage
   add constraint FK_BUSINESS_CHILDPACKAGES foreign key (ParentPackageID)
      references BusinessPackage (ID)
go

alter table BusinessProperty
   add constraint FK_BUSINESS_CLASS_PROPERTY foreign key (CLASSID)
      references BusinessClass (ID)
go

alter table BusinessProperty
   add constraint FK_BUSINESS_PROPTYPE_CLASS foreign key (ClassTypeID)
      references BusinessClass (ID)
go

alter table BusinessProperty
   add constraint FK_BUSINESS_PROPTYPE_ENUM foreign key (EnumTypeID)
      references BusinessEnum (ID)
go

alter table BusinessProperty
   add constraint FK_BUSINESS_DATATYPE_PROPERTY foreign key (DataTypeID)
      references BusinessDataType (ID)
go

alter table BusinessRole
   add constraint FK_BUSINESS_PACKAGE_ROLE foreign key (PACKAGEID)
      references BusinessPackage (ID)
go

alter table BusinessUser
   add constraint FK_BUSINESS_PACKAGE_USER foreign key (PACKAGEID)
      references BusinessPackage (ID)
go

alter table BusinessUserRole
   add constraint FK_BUSINESS_USER_ROLES foreign key (USERID)
      references BusinessUser (ID)
go

alter table BusinessUserRole
   add constraint FK_BUSINESS_ROLE_USERS foreign key (ROLEID)
      references BusinessRole (ID)
go

