using System;

namespace xray
{
	public class PollingProber
	{
		public readonly int timeout, interval;
	
		public PollingProber(int timeout, int proberInterval){
			this.timeout = timeout;
			this.interval = proberInterval;
		}
			
			
		public Boolean check(params Probe[] probes)
		{
			var startedProbing = DateTime.Now.Ticks;
				
			for(;;){
				var check = check_and_match_probes(probes);
				if (check) return true;
				var hasBeenProbingFor = ticks_to_milliseconds(DateTime.Now.Ticks - startedProbing);
				if (hasBeenProbingFor > timeout) return false;
				System.Threading.Thread.Sleep(interval);
			}
			
		}
		
		private int ticks_to_milliseconds(long ticks){ return (int)(ticks / 10000); }
		
		private Boolean check_and_match_probes(Probe[] probes)
		{
			foreach (var probe in probes)
			{
				if (!probe.probeAndMatch()) return false;
			}
			return true;
		}
	}
}

