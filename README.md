# Active study

Open source CRM system for teachers and students. The main goal of the project is decreasing of routine work for teachers and providing lead experience in education process to all students. Way the system is going to do this:

- Lecture material provided by best teachers
- Online homework for material understanding checks
- Progress monitoring
- Individual support in class

## Development

### Remove school script

```js
function removeSchool(schoolId) {
    db.audit.deleteMany({'data.schoolId': schoolId});
    db.schoolSubjects.deleteMany({'schoolId': schoolId});

    db.classes.deleteMany({'schoolId': ObjectId(schoolId)});
    db.students.deleteMany({'schoolId': ObjectId(schoolId)});
    db.teachers.deleteMany({'schoolId': ObjectId(schoolId)});

    var schoolRoles = db.roleClaims.find({'ClaimType': 'school', 'ClaimValue': schoolId}).toArray();
    for (var i = 0; i < schoolRoles.length; i++) {
        db.userRoles.deleteMany({'RoleId': schoolRoles[i].RoleId});
        db.roles.deleteOne({'_id': schoolRoles[i].RoleId});
        db.roleClaims.deleteOne({'RoleId': schoolRoles[i].RoleId});
    }

    var schedulePeriods = db.schedulePeriods.find({'schoolId': ObjectId(schoolId)}).toArray();
    for (var i = 0; i < schedulePeriods.length; i++) {
        db.scheduleLessons.deleteMany({'periodId': schedulePeriods[i]._id});
        db.schedulePeriods.deleteOne({'_id': schedulePeriods[i]._id});
    }

    db.schools.deleteOne({'_id': ObjectId(schoolId)});
}
removeSchool('xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx');
```
