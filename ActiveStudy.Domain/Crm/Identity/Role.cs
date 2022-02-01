using System;
using System.Collections.Generic;

namespace ActiveStudy.Domain.Crm.Identity;

public record Role(string Name, IDictionary<Guid, AccessLevel> Access)
{
    public const string PrincipalName = "Principal";
    public static readonly Role Principal = new Role(PrincipalName, new Dictionary<Guid, AccessLevel>
    {
        [Sections.Teachers] = AccessLevel.Full,
        [Sections.Students] = AccessLevel.Full,
        [Sections.Classes] = AccessLevel.Full
    });

    public const string TeacherName = "Teacher";
    public static readonly Role Teacher = new Role(TeacherName, new Dictionary<Guid, AccessLevel>
    {
        [Sections.Teachers] = AccessLevel.Readonly,
        [Sections.Students] = AccessLevel.Full,
        [Sections.Classes] = AccessLevel.Readonly
    });

    public const string StudentName = "Student";
    public static readonly Role Student = new Role(StudentName, new Dictionary<Guid, AccessLevel>
    {
        [Sections.Teachers] = AccessLevel.NoAccess,
        [Sections.Students] = AccessLevel.NoAccess,
        [Sections.Classes] = AccessLevel.NoAccess
    });

    public const string RelativeName = "Relative";
    public static readonly Role Relative = new Role(RelativeName, new Dictionary<Guid, AccessLevel>
    {
        [Sections.Teachers] = AccessLevel.Readonly,
        [Sections.Students] = AccessLevel.NoAccess,
        [Sections.Classes] = AccessLevel.NoAccess
    });
}
