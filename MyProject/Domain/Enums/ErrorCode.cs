using MyProject.Helpers.Attributes;

namespace MyProject.Domain.Enums
{
    public enum ErrorCode
    {
        [DescriptionAnnotation("none", "none")]
        None = 0,

        [DescriptionAnnotation("Validation Errors", "Validation Errors")]
        ValidationErrors = 1,

        [DescriptionAnnotation("Driver Not Found", "The specified driver was not found")]
        DriverNotFound = 2,

        [DescriptionAnnotation(" ID Required", " ID is required")]
        IdRequired = 3,

        [DescriptionAnnotation("Invalid ID Format", "Invalid ID format")]
        InvalidIdFormat = 4
    }
}
