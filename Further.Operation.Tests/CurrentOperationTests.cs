using FluentResults;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
//using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Further.Operation.Tests
{
    public class CurrentOperationTests : OperationIntegrationTestBase
    {
        private readonly ICurrentOperation _currentOperation;
        private readonly ICurrentOperationAccessor _currentOperationAccessor;

        public CurrentOperationTests()
        {
            _currentOperation = this.GetRequiredService<ICurrentOperation>();
            _currentOperationAccessor = this.GetRequiredService<ICurrentOperationAccessor>();
        }

        [Fact]
        public async Task ModifyAsync_Should_Modify_Current_Operation()
        {
            // Arrange
            var operationId = Guid.NewGuid();



            _currentOperationAccessor.Initial(operationId);

            // Act
            await _currentOperation.SaveAsync(info => info.Name = "Modified");

            // Assert
            _currentOperation.Name.ShouldBe("Modified");
        }

    }
}
