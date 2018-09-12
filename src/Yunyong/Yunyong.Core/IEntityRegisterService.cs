using Microsoft.EntityFrameworkCore;

namespace Yunyong.Core
{
    /// <summary>
    ///     IEntityRegisterService
    /// </summary>
    public interface IEntityRegisterService
    {
        /// <summary>
        ///     Registers the entities.
        /// </summary>
        /// <param name="builder">The builder.</param>
        void RegisterEntities(ModelBuilder builder);
    }
}