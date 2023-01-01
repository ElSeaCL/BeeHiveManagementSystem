using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BeeHiveManagementSystem
{
    class Bee
    {
        public string Job { get; private set; }

        public virtual float CostPerShift { get; }

        public Bee(string job)
        {
            this.Job = job;
        }

        public void WorkTheNextShift() 
        {
            bool hayMiel = HoneyVault.ConsumeHoney(CostPerShift);
            if (hayMiel) DoJob();
        }
        protected virtual void DoJob() { /* por implemntar en cada subclase */ }
    }

    class Queen : Bee
    {
        private const float EGGS_PER_SHIFT = 0.45f;
        private const float HONEY_PER_UNASSIGNED_WORKER = 0.5f;

        private Bee[] workers = new Bee[0];
        private float unassignedWorkers = 3;
        private float eggs = 0;

        public string StatusReport { get; private set; }
        public override float CostPerShift { get { return 2.15f; } }

        public Queen() : base("Queen") 
        {
            AssigBee("Nectar Collector");
            AssigBee("Honey Manufacturer");
            AssigBee("Egg Care");
        }

        public void AssigBee(string job) 
        {
            switch(job)
            {
                case "Egg Care":
                    AddWorker(new EggCare(this));
                    break;
                case "Nectar Collector":
                    AddWorker(new NectarCollertor());
                    break;
                case "Honey Manufacturer":
                    AddWorker(new HoneyManufacturer());
                    break;
            }

            UpdateStatusReport();
        }
        public void CareForEggs(float eggsToConvert) 
        {
            if (eggsToConvert <= eggs)
            {
                eggs -= eggsToConvert;
                unassignedWorkers += eggsToConvert;
            }
        }
        protected override void DoJob() 
        {
            // La reina agrega lo huevo
            eggs += EGGS_PER_SHIFT;

            // llama a los zanganos a trabajar
            foreach (Bee bee in workers)
            {
                bee.WorkTheNextShift();
            }

            // alimenta a las abejas no asignadas
            HoneyVault.ConsumeHoney(HONEY_PER_UNASSIGNED_WORKER * unassignedWorkers);

            // Actualiza el reporte
            UpdateStatusReport();
        }

        private void AddWorker(Bee worker) 
        {
            if (unassignedWorkers >= 1)
            {
                unassignedWorkers -= 1;
                Array.Resize(ref workers, workers.Length + 1);
                workers[workers.Length - 1] = worker;
            }
        }

        private string WorkerStatus(string job)
        {
            int count = 0;
            foreach (Bee worker in workers)
                if (worker.Job == job) count++;
            string s = "s";
            if (count == 1) s = "";
            return $"{count} {job} bee{s}";
        }

        private void UpdateStatusReport()
        {
            StatusReport = $"Vault report:\n{HoneyVault.StatusReport}\n" +
            $"\nEgg count: {eggs:0.0}\nUnassigned workers: {unassignedWorkers:0.0}\n" +
            $"{WorkerStatus("Nectar Collector")}\n{WorkerStatus("Honey Manufacturer")}" +
            $"\n{WorkerStatus("Egg Care")}\nTOTAL WORKERS: {workers.Length}";
        }
    }

    class NectarCollertor : Bee 
    {
        private const float NECTAR_COLLECTED_PER_SHIFT = 33.25f;
        public override float CostPerShift { get { return 1.95f; } }

        public NectarCollertor() : base("Nectar Collector") { }

        protected override void DoJob()
        {
            HoneyVault.CollectNectar(NECTAR_COLLECTED_PER_SHIFT);
        }
    }

    class HoneyManufacturer : Bee 
    {
        private const float NECTAR_PROCESSED_PER_SHIFT = 33.15f;
        public override float CostPerShift { get { return 1.7f; } }

        public HoneyManufacturer() : base("Honey Manufacturer") { }

        protected override void DoJob()
        {
            HoneyVault.ConverNectarToHoney(NECTAR_PROCESSED_PER_SHIFT);
        }
    }

    class EggCare : Bee 
    {
        public const float CARE_PROGRESS_PER_SHIFT = 0.15f;
        public override float CostPerShift { get { return 1.35f; } }
        private Queen queen;

        public EggCare(Queen queen) : base("Egg Care") 
        {
            this.queen = queen;
        }

        protected override void DoJob()
        {
            queen.CareForEggs(CARE_PROGRESS_PER_SHIFT);
        }
    }
}
