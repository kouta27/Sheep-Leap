namespace RankingSystem
{
    public interface IRankingReceiver
    {
        public void OnRankingLoadSuccess(RankingData[] datas);
        public void OnRankingLoadError();

        public void OnRankingPostSuccess();
        public void OnRankingPostError();
    }
}
