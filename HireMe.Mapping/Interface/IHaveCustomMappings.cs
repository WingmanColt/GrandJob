using AutoMapper;

namespace HireMe.Mapping.Interface
{
    using AutoMapper;

    public interface IHaveCustomMappings
    {
        void CreateMappings(IProfileExpression configuration);
    }
}
