using ProjectLab.Models;

namespace ProjectLab.Services
{
    public class BucketService
    {
        private readonly LabContext _context;
        public BucketService(LabContext _dbContext) 
        { 
            _context = _dbContext;
        }

        public int addDeployedScript(Bucket bucket)
        {
            if(bucket == null)
            {
                return 0;
            }
            _context.Buckets.Add(bucket);
            _context.SaveChanges();
            return bucket.BucketId;
        }
    }
}
