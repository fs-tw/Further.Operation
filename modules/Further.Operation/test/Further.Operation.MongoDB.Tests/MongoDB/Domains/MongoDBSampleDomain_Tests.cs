using Further.Operation.Samples;
using Xunit;

namespace Further.Operation.MongoDB.Domains;

[Collection(MongoTestCollection.Name)]
public class MongoDBSampleDomain_Tests : SampleManager_Tests<OperationMongoDbTestModule>
{

}
