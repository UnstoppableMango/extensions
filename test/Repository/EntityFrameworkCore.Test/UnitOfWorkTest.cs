using Microsoft.EntityFrameworkCore;
using Moq;

namespace KG.Data.EntityFrameworkCore
{
    public class UnitOfWorkTest
    {
        private readonly Mock<DbContextOptions> _options = new Mock<DbContextOptions>();

        private readonly UnitOfWork _unitOfWork;

        public UnitOfWorkTest()
        {
            _unitOfWork = new UnitOfWork(_options.Object);
        }
    }
}
