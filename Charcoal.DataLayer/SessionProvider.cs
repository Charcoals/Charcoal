using Raven.Client;
using Raven.Client.Embedded;

namespace Charcoal.DataLayer {
    public interface ISessionProvider
    {
        IDocumentSession Session();
    }

    public class SessionProvider : ISessionProvider
    {
        
        private static readonly IDocumentStore store;

        static SessionProvider()
        {
            //use for unit tests, eventually need to inject this
            store = new EmbeddableDocumentStore
                        {RunInMemory = true }; //, DataDirectory = "data", DefaultDatabase = "charcoal"}
            store.Initialize();
        }
        public IDocumentSession Session()
        {
            return store.OpenSession();
        }
    }
}
