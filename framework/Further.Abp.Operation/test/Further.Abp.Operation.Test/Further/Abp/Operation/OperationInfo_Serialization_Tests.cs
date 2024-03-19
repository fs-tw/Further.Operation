using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Further.Abp.Operation
{
    public class OperationInfo_Serialization_Tests : OperationTestBase
    {
        Volo.Abp.Json.IJsonSerializer jsonSerializer;

        public OperationInfo_Serialization_Tests()
        {
            jsonSerializer = ServiceProvider.GetRequiredService<Volo.Abp.Json.IJsonSerializer>();
        }

        [Fact]
        public void Should_Deserialize_And_Serialize()
        {
            Result result = Result.Ok();
            result.WithSuccess("Test");
            var r = new Error("ErrorWithMetadata");
            r.Metadata.Add("11", "22");
            result.WithError(r);

            var operationInfo = new OperationInfo(Guid.NewGuid(), "Test", "Test", result,
                new List<OperationOwnerInfo>()
                {
                    new OperationOwnerInfo(){EntityType ="112",EntityId=Guid.Empty,MetaData=new Dictionary<string, object>(){ {"1",1 }, { "2", 1 }, { "3", result } } },
                    new OperationOwnerInfo(){EntityType ="113",EntityId=Guid.Empty,MetaData=new Dictionary<string, object>()},
                    new OperationOwnerInfo(){EntityType ="114",EntityId=Guid.Empty,MetaData=new Dictionary<string, object>()}

                }, 0);

            var text = jsonSerializer.Serialize(operationInfo);


            var obj = jsonSerializer.Deserialize<OperationInfo>(text);

            var r2 = jsonSerializer.Serialize(obj);
            //var data = JsonSerializer.Deserialize(
            //        JsonSerializer.Serialize(new MessageNotificationData("Hello World!")),
            //        typeof(MessageNotificationData)
            //    ) as MessageNotificationData;

            //Assert.NotNull(data);
            //data.Message.ShouldBe("Hello World!");
        }

    }
}
