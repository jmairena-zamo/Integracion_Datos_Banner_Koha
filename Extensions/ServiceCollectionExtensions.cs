using Integracion_Datos_Banner_Koha.Hangfire.Filters;
using Integracion_Datos_Banner_Koha.Services.Auth;
using Integracion_Datos_Banner_Koha.Services.Auth.Interfaces;



namespace Integracion_Datos_Banner_Koha.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //------- HangFire
            services.AddTransient<HangfireDashboardAuthFilter>();
            ////------- Http Client
            //services.AddHttpClient<ITokenBannerServices, TokenBannerServices>();
            //services.AddHttpClient<IBannerServices, BannerServices>();
            //services.AddHttpClient<IApiSpecificationsServices, ApiSpecificationsServices>();

            //////------- GraphQL
            ////services.AddTransient<IGraphQLProxyServices, GraphQLProxyServices>();

            ////------- Message Queues
            //services.AddScoped<IMessageQueuesServices, MessageQueuesServices>();

            ////------- Custom API
            //services.AddTransient<IExecuteCustomApiServices, ExecuteCustomApiServices>();
            //services.AddTransient<ITableColumnsServices, TableColumnsServices>();
            //services.AddTransient<ITableConstraintsServices, TableConstraintsServices>();
            //services.AddTransient<ITableDataServices, TableDataServices>();
            //services.AddTransient<IStudentChargesServices, StudentChargesServices>();
            //services.AddTransient<IFinancialIntegrationOrdersServices, FinancialIntegrationOrdersServices>();
            //services.AddTransient<IFinancialIntegrationPaymentsServices, FinancialIntegrationPaymentsServices>();

            ////------- Banner
            //services.AddTransient<IAuthServices, AuthServices>();
            //services.AddTransient<ITokenBannerServices, TokenBannerServices>();
            //services.AddTransient<IBannerServices, BannerServices>();
            //services.AddTransient<IPersonsServices, PersonsServices>();
            //services.AddTransient<IAddressTypesServices, AddressTypesServices>();
            //services.AddTransient<IGeographicAreasServices, GeographicAreasServices>();
            //services.AddTransient<IAddressesServices, AddressesServices>();
            //services.AddTransient<IPersonNameTypesServices, PersonNameTypesServices>();
            //services.AddTransient<IGendersServices, GendersServices>();
            //services.AddTransient<IGenderIdentitiesServices, GenderIdentitiesServices>();
            //services.AddTransient<IPersonalPronounsServices, PersonalPronounsServices>();
            //services.AddTransient<IReligionsServices, ReligionsServices>();
            //services.AddTransient<IEthnicitiesServices, EthnicitiesServices>();
            //services.AddTransient<IStudentAccountDetailChargesPaymentsServices, StudentAccountDetailChargesPaymentsServices>();
            //services.AddTransient<IRacesServices, RacesServices>();
            //services.AddTransient<IMaritalStatusesServices, MaritalStatusesServices>();
            //services.AddTransient<ICitizenshipStatusesServices, CitizenshipStatusesServices>();
            //services.AddTransient<IAlternativeCredentialTypesServices, AlternativeCredentialTypesServices>();
            //services.AddTransient<IStudentDetailCodeControlsServices, StudentDetailCodeControlsServices>();
            //services.AddTransient<IInterestsServices, InterestsServices>();
            //services.AddTransient<IPhoneTypesServices, PhoneTypesServices>();
            //services.AddTransient<IEmailTypesServices, EmailTypesServices>();
            //services.AddTransient<IAdmissionApplicationsServices, AdmissionApplicationsServices>();
            //services.AddTransient<IAdmissionApplicationsSubmissionsServices, AdmissionApplicationsSubmissionsServices>();
            //services.AddTransient<IAcademicPeriodsServices, AcademicPeriodsServices>();
            //services.AddTransient<IAcademicProgramsServices, AcademicProgramsServices>();
            //services.AddTransient<IAdmissionPopulationsServices, AdmissionPopulationsServices>();
            //services.AddTransient<IStudentAcademicProgramsServices, StudentAcademicProgramsServices>();
            //services.AddTransient<IResidencyTypesServices, ResidencyTypesServices>();
            //services.AddTransient<ISitesServices, SitesServices>();
            //services.AddTransient<ICountriesServices, CountriesServices>();
            //services.AddTransient<IEmailAddressServices, EmailAddressServices>();
            //services.AddTransient<IStudentsServices, StudentsServices>();
            //services.AddTransient<IStudentClassificationsServices, StudentClassificationsServices>();
            //services.AddTransient<IAcademicLevelsServices, AcademicLevelsServices>();
            //services.AddTransient<IAdministrativePeriodsServices, AdministrativePeriodsServices>();
            //services.AddTransient<IStudentTypesServices, StudentTypesServices>();
            //services.AddTransient<IEmailAddressesServices, EmailAddressesServices>();
            //services.AddTransient<IInstructorsServices, InstructorsServices>();
            //services.AddTransient<IGpaAcademicStandingsServices, GpaAcademicStandingsServices>();
            //services.AddTransient<IStudentAcademicHistoryCourseDetailsServices, StudentAcademicHistoryCourseDetailsServices>();
            //services.AddTransient<IFacultyInformationBaseDetailsServices, FacultyInformationBaseDetailsServices>();
            //services.AddTransient<IFacultyInformationContractsServices, FacultyInformationContractsServices>();
            //services.AddTransient<IFacultyInformationCollegesAndDepartmentsServices, FacultyInformationCollegesAndDepartmentsServices>();
            //services.AddTransient<IFacultyInformationAttributesServices, FacultyInformationAttributesServices>();
            //services.AddTransient<IAdditionalStudentInformationCohortServices, AdditionalStudentInformationCohortServices>();
            //services.AddTransient<IAdditionalStudentInformationAttributeServices, AdditionalStudentInformationAttributeServices>();
            //services.AddTransient<ITermControlPartOfTermAndWebRegistrationControlsServices, TermControlPartOfTermAndWebRegistrationControlsServices>();
            //services.AddTransient<ITermControlRegistrationServices, TermControlRegistrationServices>();
            //services.AddTransient<IMedicalInformationServices, MedicalInformationServices>();
            //services.AddTransient<IUserIdentityProfilesServices, UserIdentityProfilesServices>();
            //services.AddTransient<IPersonHoldsServices, PersonHoldsServices>();
            //services.AddTransient<IMailsServices, MailsServices>();
            //services.AddTransient<IRelationshipTypesServices, RelationshipTypesServices>();
            //services.AddTransient<ITelephonesServices, TelephonesServices>();
            //services.AddTransient<IMultipleAdvisorsServices, MultipleAdvisorsServices>();
            //services.AddTransient<IIntegrationPartnerRulesServices, IntegrationPartnerRulesServices>();
            //services.AddTransient<IEmergencyContactService, EmergencyContactServices>();
            //services.AddTransient<IAdmissionsDecisionProcessingServices, AdmissionsDecisionProcessingServices>();
            //services.AddTransient<IInternationalInformationServices, InternationalInformationServices>();
            //services.AddTransient<IHighSchoolInformationHighSchoolDetailsServices, HighSchoolInformationHighSchoolDetailsServices>();
            //services.AddTransient<IPriorCollegesAndDegreesServices, PriorCollegesAndDegreesServices>();
            //services.AddTransient<ITestScoreInformationServices, TestScoreInformationServices>();
            //services.AddTransient<IPriorCollegeMajorsMinorsConcentrationsServices, PriorCollegeMajorsMinorsConcentrationsServices>();
            //services.AddTransient<IPriorCollegePriorCollegeAndDegreeServices, PriorCollegePriorCollegeAndDegreeServices>();
            //services.AddTransient<IProcessSubmissionControlsServices, ProcessSubmissionControlsServices>();
            //services.AddTransient<IAdmissionApplicationTypesServices, AdmissionApplicationTypesServices>();
            //services.AddTransient<IAdmissionsApplicationContactsCohortsAttributesServices, AdmissionsApplicationContactsCohortsAttributesServices>();
            //services.AddTransient<IIdentificationCurrentIdentificationServices, IdentificationCurrentIdentificationServices>();
            //services.AddTransient<IPersonHoldTypesServices, PersonHoldTypesServices>();
            //services.AddTransient<IHoldInformationServices, HoldInformationServices>();
            //services.AddTransient<IAdmissionDecisionsServices, AdmissionDecisionsServices>();
            //services.AddTransient<IAdmissionsApplicationContactsCohortsAttributesServices2, AdmissionsApplicationContactsCohortsAttributesServices2>();
            //services.AddTransient<ISourceBackgroundInstitutionCodes, SourceBackgroundInstitutionCodes>();
            //services.AddTransient<ISourceOrBackgroundInstitution, SourceOrBackgroundInstitution>();
            //services.AddTransient<ISectionMeetingsServices, SectionMeetingsServices>();
            //services.AddTransient<IApiSpecificationsServices, ApiSpecificationsServices>();
            //services.AddTransient<ISectionInstructorsServices, SectionInstructorsServices>();
            //services.AddTransient<IAdditionalStudentInformationAttributeEndServices, AdditionalStudentInformationAttributeEndServices>();
            //services.AddTransient<IProgramRequirementsProgramAreaAttachmentsServices, ProgramRequirementsProgramAreaAttachmentsServices>();
            //services.AddTransient<IAreaRequirementsAreaCourseAttachmentsServices, AreaRequirementsAreaCourseAttachmentsServices>();
            //services.AddTransient<ISectionAssignedInstructorsServices, SectionAssignedInstructorsServices>();
            //services.AddTransient<IInstructionalMethodsServices, InstructionalMethodsServices>();
            //services.AddTransient<IInstructionalDeliveryMethodsServices, InstructionalDeliveryMethodsServices>();
            //services.AddTransient<IThirdPartyAccessAuditPinHistoryServices, ThirdPartyAccessAuditPinHistoryServicess>();
            //services.AddTransient<ICoursesServices, CoursesServices>();
            //services.AddTransient<ISectionTitleTypesServices, SectionTitleTypesServices>();
            //services.AddTransient<ISectionsServices, SectionsServices>();
            //services.AddTransient<ITermControlBasePartOfTermsServices, TermControlBasePartOfTermsServices>();
            //services.AddTransient<ISubjectsServices, SubjectsServices>();
            //services.AddTransient<ICourseTitleTypesServices, CourseTitleTypesServices>();
            //services.AddTransient<IDormRoomAndMealApplicationServices, DormRoomAndMealApplicationServices>();
            //services.AddTransient<IProgramaLiderService, ProgramaLiderService>();
            //services.AddTransient<IDatabaseQueryPreviewsService, DatabaseQueryPreviewsServices>();
            //services.AddTransient<IRegistrationRegisterServices, RegistrationRegisterServices>();

            ////-------ZA - AccountReceivable
            //services.AddTransient<IEstadoCuentaService, EstadoCuentaService>();
            //services.AddTransient<IAccountDetailReviewFormServices, AccountDetailReviewFormServices>();
            //services.AddTransient<IAccountingRulesServices, AccountingRulesServices>();

            ////-------ZA - Persons
            //services.AddTransient<ICorreoInstitucionalServices, CorreoInstitucionalServices>();
            //services.AddTransient<IPersonsInfoServices, PersonsInfoServices>();
            //services.AddTransient<Services.Persons.Interfaces.IStudentEmailValidationServices, Services.Persons.StudentEmailValidationServices>();
            //services.AddTransient<IEmployeeEmailValidationServices, EmployeeEmailValidationServices>();

            ////------- ZA - ReplicateDatabase
            //services.AddTransient<IBulkDataServices, BulkDataServices>();
            //services.AddTransient<ICreateTableQueryServices, CreateTableServices>();
            //services.AddTransient<ITableDependenciesServices, TableDependenciesServices>();
            //services.AddTransient<IExecuteQueryBannerServices, ExecuteQueryBannerServices>();
            //services.AddScoped<IExecuteQueryLocalServices, ExecuteQueryLocalServices>();
            //services.AddTransient<IQueriesBannerServices, QueriesBannerServices>();
            //services.AddTransient<IQueriesLocalServices, QueriesLocalServices>();
            //services.AddTransient<IQueriesServices, QueriesServices>();
            //services.AddTransient<IConvertDataServices, ConvertDataServices>();
            //services.AddTransient<ICustomApiServices, CustomApiServices>();
            //services.AddTransient<ITableDataBackgroundJob, TableDataBackgroundJob>();
            //services.AddTransient<IForeignKeyServices, ForeignKeyServices>();

            ////------- ZA - Satellite
            ////SIADES
            //services.AddTransient<IAccionesEstudiantilesFaltasServices, AccionesEstudiantilesFaltasServices>();
            //services.AddTransient<ISearchesServices, SearchesServices>();
            ////Permisos de Salida
            //services.AddTransient<ITiposSalidaServices, TiposSalidaServices>();
            //services.AddTransient<ISolicitudSalidaServices, SolicitudSalidaServices>();
            //services.AddTransient<ISolicitudSalidaHelperServices, SolicitudSalidaHelperServices>();
            //services.AddTransient<IPlanificacionSalidaServices, PlanificacionSalidaServices>();
            //services.AddTransient<ISolicitudSalidaEmailServices, SolicitudSalidaEmailServices>();
            ////Carnet
            //services.AddTransient<ICarnetEstudianteServices, CarnetEstudianteServices>();
            ////Housing
            //services.AddTransient<IZonasServices, ZonasServices>();
            //services.AddTransient<IResidenciasServices, ResidenciasServices>();
            //services.AddTransient<IEdificiosZonaServices, EdificiosZonaServices>();
            //services.AddTransient<IIvesEdificioServices, IvesEdificioServices>();
            //services.AddTransient<IIfisEdificioServices, IfisEdificioServices>();
            ////Disciplina
            //services.AddTransient<ICausalSancionServices, CausalSancionServices>();
            ////Pago Online
            //services.AddTransient<ILastStudentTransactionService, LastStudentTransactionService>();
            ////fomrs
            //services.AddTransient<IFormsServices, FormsServices>();
            ////students
            //services.AddTransient<IStudentProfileService, StudentProfileService>();
            ////Schedules
            //services.AddTransient<ISchedulesServices, SchedulesServices>();
            ////Edusign - API
            //services.AddTransient<IEdusignServices, EdusignServices>();
            //services.AddTransient<IEdusignApiV3.IStudentsServices, EdusignApiV3.StudentsServices>();
            //services.AddTransient<IEdusignApiV1.IStudentsServices, EdusignApiV1.StudentsServices>();
            //services.AddTransient<IEdusignApiV3.ICoursesServices, EdusignApiV3.CoursesServices>();

            ////Edusign - Integration
            //services.AddTransient<IStudentsDBServices, StudentsDBServices>();
            //services.AddTransient<IStudentsDownloadServices, StudentsDownloadServices>();
            //services.AddTransient<IProcessAcademicSanctionsServices, ProcessAcademicSanctionsServices>();

            ////Calendar
            //services.AddTransient<ICalendarServices, CalendarServices>();

            ////------- Reistros Academicos
            //services.AddTransient<IHistorialAcademicoService, HistorialAcademicoServices>();
            //services.AddTransient<ICalculoAnioCarreraCAPP, CalculoAnioCarreraCAPP>();
            //services.AddTransient<IEstadoCuentaAspiranteService, EstadoCuentaAspiranteServices>();
            //services.AddTransient<IAdditionalInformationServices, AdditionalInformationServices>();
            //services.AddTransient<IActiveCareersServices, ActiveCareersServices>();
            //services.AddTransient<IStudentActiveServices, StudentActiveServices>();
            //services.AddTransient<IStudentProgramsServices, StudentProgramsServices>();
            //services.AddTransient<IStudentGoamediServices, StudentGoamediServices>();
            //services.AddTransient<IStudentPersonalInfoServices, StudentPersonalInfoServices>();
            //services.AddTransient<Services.Student.RegistrosAcademicos.Interfaces.IStudentEmailValidationServices, Services.Student.RegistrosAcademicos.StudentEmailValidationServices>();
            //services.AddTransient<IConcentratorProgramsAtributsesRulesServices, ConcentratorProgramsAtributsesRulesServices>();
            //services.AddTransient<IInscriptionsServices, InscriptionsServices>();
            ////------- Instructor
            //services.AddTransient<IInstructorCoursesServices, InstructorCoursesServices>();


            ////------- Data Connect
            //services.AddTransient<IPaymentServices, PaymentServices>();

            ////------- ERP Integration
            //services.AddTransient<IIntegrationERPServices, IntegrationERPServices>();

            ////-------Academic
            //services.AddTransient<INrcAHConcentratorServices, NrcAHConcentratorServices>();
            //services.AddTransient<INRCAHLocalServices, NRCAHLocalServices>();


            return services;
        }
    }
}
