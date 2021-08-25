using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActiveStudy.Domain
{
    public interface IAuditStorage
    {
        public Task LogAsync(AuditItem item);
        public Task<IEnumerable<AuditItem>> SearchAnyAsync(IDictionary<string, string> filter);
    }

    public class AuditItem
    {
        public Guid ActionId { get; }
        public IDictionary<string, string> Data { get; }

        public DateTime Time { get; }
        public User User { get; }

        public AuditItem(Guid actionId, IDictionary<string, string> data, User user)
        {
            ActionId = actionId;
            User = user;
            Data = data;

            Time = DateTime.UtcNow;
        }
        
        public static Guid TeacherCreated = new Guid("9B4D025A-ECF5-4EAE-A3BE-083DC753C8CA");
        public static Guid TeacherRemoved = new Guid("B13B1A50-EB02-4ED7-A741-CA6B1DB49AE1");
        public static Guid TeacherAddedToClass = new Guid("5F81919F-1276-4CCA-9C58-492B31AEB76D");
        public static Guid TeacherInvited = new Guid("0D5D2438-1A31-4C95-8DD8-3429DCD4FEC0");

        public static Guid SchoolCreated = new Guid("87C411CD-FF23-4A05-AA78-F36A7685AEE8");

        public static Guid StudentCreated = new Guid("580C80B6-1A10-4B65-96DF-212633B5E761");
        public static Guid StudentAddedToClass = new Guid("1B57C7BF-A512-43C6-8D20-A33C1C9A0DEA");

        public static Guid ClassCreated = new Guid("A68D1F74-FEC5-4030-9603-2C4079D92BC7");

        public static Guid RelativeCreated = new Guid("CE395B46-13CA-4D54-AB77-4EA30A74D822");
        public static Guid RelativeAddedToStudent = new Guid("925F22E9-801D-483F-BCAF-C8D51CAEC575");
    }
    
    public static class AuditStorageExtensions
    {
        public static async Task LogTeacherCreatedAsync(this IAuditStorage auditStorage,
            string schoolId, string schoolTitle,
            string teacherId, string teacherName,
            User user)
        {
            await auditStorage.LogAsync(new AuditItem(AuditItem.TeacherCreated,
                new Dictionary<string, string>
                {
                    ["schoolId"] = schoolId,
                    ["schoolTitle"] = schoolTitle,
                    ["teacherId"] = teacherId,
                    ["teacherName"] = teacherName
                },
                user));
        }

        public static async Task LogTeacherInvitedAsync(this IAuditStorage auditStorage,
            string schoolId, string schoolTitle,
            string teacherId, string teacherName,
            User user)
        {
            await auditStorage.LogAsync(new AuditItem(AuditItem.TeacherInvited,
                new Dictionary<string, string>
                {
                    ["schoolId"] = schoolId,
                    ["schoolTitle"] = schoolTitle,
                    ["teacherId"] = teacherId,
                    ["teacherName"] = teacherName
                },
                user));
        }

        public static async Task LogTeacherRemovedAsync(this IAuditStorage auditStorage,
            string schoolId, string schoolTitle,
            string teacherId, string teacherName,
            User user)
        {
            await auditStorage.LogAsync(new AuditItem(AuditItem.TeacherRemoved,
                new Dictionary<string, string>
                {
                    ["schoolId"] = schoolId,
                    ["schoolTitle"] = schoolTitle,
                    ["teacherId"] = teacherId,
                    ["teacherName"] = teacherName
                },
                user));
        }

        public static async Task LogSchoolCreateAsync(this IAuditStorage auditStorage,
            string schoolId, string schoolTitle,
            User user)
        {
            await auditStorage.LogAsync(new AuditItem(AuditItem.SchoolCreated,
                new Dictionary<string, string>
                {
                    ["schoolId"] = schoolId,
                    ["schoolTitle"] = schoolTitle
                },
                user));
        }

        public static async Task LogStudentCreateAsync(this IAuditStorage auditStorage,
            string schoolId, string schoolTitle,
            string studentId, string studentName,
            User user)
        {
            await auditStorage.LogAsync(new AuditItem(AuditItem.StudentCreated,
                new Dictionary<string, string>
                {
                    ["schoolId"] = schoolId,
                    ["schoolTitle"] = schoolTitle,
                    ["studentId"] = studentId,
                    ["studentName"] = studentName
                },
                user));
        }

        public static async Task LogStudentAddedToClassAsync(this IAuditStorage auditStorage,
            string schoolId, string schoolTitle,
            string studentId, string studentName,
            string classId, string classTitle,
            User user)
        {
            await auditStorage.LogAsync(new AuditItem(AuditItem.StudentAddedToClass,
                new Dictionary<string, string>
                {
                    ["schoolId"] = schoolId,
                    ["schoolTitle"] = schoolTitle,
                    ["studentId"] = studentId,
                    ["studentName"] = studentName,
                    ["classId"] = classId,
                    ["classTitle"] = classTitle
                },
                user));
        }

        public static async Task LogClassCreatedAsync(this IAuditStorage auditStorage,
            string schoolId, string schoolTitle,
            string classId, string classTitle,
            User user)
        {
            await auditStorage.LogAsync(new AuditItem(AuditItem.ClassCreated,
                new Dictionary<string, string>
                {
                    ["schoolId"] = schoolId,
                    ["schoolTitle"] = schoolTitle,
                    ["classId"] = classId,
                    ["classTitle"] = classTitle
                },
                user));
        }

        public static async Task LogRelativeCreatedAsync(this IAuditStorage auditStorage,
            string schoolId, string schoolTitle,
            string relativeId, string relativeName,
            User user)
        {
            await auditStorage.LogAsync(new AuditItem(AuditItem.RelativeCreated,
                new Dictionary<string, string>
                {
                    ["schoolId"] = schoolId,
                    ["schoolTitle"] = schoolTitle,
                    ["relativeId"] = relativeId,
                    ["relativeName"] = relativeName
                },
                user));
        }

        public static async Task LogRelativeAddedToStudentAsync(this IAuditStorage auditStorage,
            string schoolId, string schoolTitle,
            string studentId, string studentName,
            string relativeId, string relativeName,
            User user)
        {
            await auditStorage.LogAsync(new AuditItem(AuditItem.RelativeAddedToStudent,
                new Dictionary<string, string>
                {
                    ["schoolId"] = schoolId,
                    ["schoolTitle"] = schoolTitle,
                    ["studentId"] = studentId,
                    ["studentName"] = studentName,
                    ["relativeId"] = relativeId,
                    ["relativeName"] = relativeName
                },
                user));
        }

        public static async Task LogTeacherAddedToClassAsync(this IAuditStorage auditStorage,
            string schoolId, string schoolTitle,
            string teacherId, string teacherName,
            string classId, string classTitle,
            User user)
        {
            await auditStorage.LogAsync(new AuditItem(AuditItem.TeacherAddedToClass,
                new Dictionary<string, string>
                {
                    ["schoolId"] = schoolId,
                    ["schoolTitle"] = schoolTitle,
                    ["teacherId"] = teacherId,
                    ["teacherName"] = teacherName,
                    ["classId"] = classId,
                    ["classTitle"] = classTitle
                },
                user));
        }
    }
}