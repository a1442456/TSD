using System;
using System.Threading.Tasks;
using Cen.Common.Sync.Interfaces;

namespace Cen.Common.Sync.Abstract
{
    public abstract class SyncDestination<I, O>: ISyncDestination<I, O>
    {
        public async Task<long> WriteSource(
            ISyncSession session, ISyncPositionProvider positionProvider, ISyncPositionsStore positionsStore, 
            string stepEntityName, ISyncSource<I, O> syncSource, I syncParameter)
        {
            var sessionId = session.SessionId;
            if (!sessionId.HasValue)
                throw new InvalidOperationException();
            
            var countProcessed = 0;
            var progressStep = GetProgressStep();

            var currentPosition = await positionProvider.GetCurrentPosition(stepEntityName);
            var syncSessionStepId = await session.StoreSessionStepStart(stepEntityName);
            var itemsCount = await syncSource.Count(positionsStore, stepEntityName, syncParameter);
            await session.StoreSessionStepTotal(syncSessionStepId, itemsCount);

            await PreProcess();
            
            var itemsAsyncEnumerator = syncSource.AsEnumerable(positionsStore, stepEntityName, syncParameter).GetAsyncEnumerator();
            while (await itemsAsyncEnumerator.MoveNextAsync())
            {
                await WriteItem(itemsAsyncEnumerator.Current, sessionId.Value);
                countProcessed++;
                
                if (countProcessed % progressStep == 0)
                    await session.StoreSessionStepProgress(syncSessionStepId, countProcessed);
            }
            await session.StoreSessionStepProgress(syncSessionStepId, countProcessed);
            await session.StoreSessionStepFinish(syncSessionStepId);
            await positionsStore.SetPosition(stepEntityName, currentPosition);
            await PostProcess();
            
            return countProcessed;
        }
        
        protected abstract int GetProgressStep();

        protected abstract Task PreProcess();
        
        protected abstract Task WriteItem(O item, int syncSessionId);
        
        protected abstract Task PostProcess();
    }
}