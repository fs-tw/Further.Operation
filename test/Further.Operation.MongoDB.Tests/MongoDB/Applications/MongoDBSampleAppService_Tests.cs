using Further.Operation.MongoDB;
using Further.Operation.Samples;
using Xunit;

namespace Further.Operation.MongoDb.Applications;

[Collection(MongoTestCollection.Name)]
public class MongoDBSampleAppService_Tests : SampleAppService_Tests<OperationMongoDbTestModule>
{

}
