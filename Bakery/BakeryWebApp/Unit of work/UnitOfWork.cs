using BakeryWebApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryWebApp.Unit_of_work
{
    public class UnitOfWork
    {
        private string _connectionString;

        public UnitOfWork(string connectionString)
        {
            _connectionString = connectionString;
        }

        private DayPlanRepository dayPlanRepository;
        private DayProductionRepository dayProductionRepository;
        private MaterialRepository materialRepository;
        private NormRepository normRepository;
        private ProductRepository productRepository;
        private ProductMaterialRepository productMaterialRepository;
        private ProductsPlanRepository productsPlanRepository;
        private ProductsProductionRepository productsProductionRepository;
        private RawRepository rawRepository;

        public DayPlanRepository DayPlans
        {
            get
            {
                if (dayPlanRepository == null)
                    dayPlanRepository = new DayPlanRepository(_connectionString);
                return dayPlanRepository;
            }
        }

        public DayProductionRepository DayProductions
        {
            get
            {
                if (dayProductionRepository == null)
                    dayProductionRepository = new DayProductionRepository(_connectionString);
                return dayProductionRepository;
            }
        }

        public MaterialRepository Materials
        {
            get
            {
                if (materialRepository == null)
                    materialRepository = new MaterialRepository(_connectionString);
                return materialRepository;
            }
        }

        public NormRepository Norms
        {
            get
            {
                if (normRepository == null)
                    normRepository = new NormRepository(_connectionString);
                return normRepository;
            }
        }

        public ProductRepository Products
        {
            get
            {
                if (productRepository == null)
                    productRepository = new ProductRepository(_connectionString);
                return productRepository;
            }
        }

        public ProductMaterialRepository ProductMaterials
        {
            get
            {
                if (productMaterialRepository == null)
                    productMaterialRepository = new ProductMaterialRepository(_connectionString);
                return productMaterialRepository;
            }
        }

        public ProductsPlanRepository ProductsPlans
        {
            get
            {
                if (productsPlanRepository == null)
                    productsPlanRepository = new ProductsPlanRepository(_connectionString);
                return productsPlanRepository;
            }
        }

        public ProductsProductionRepository ProductsProductions
        {
            get
            {
                if (productsProductionRepository == null)
                    productsProductionRepository = new ProductsProductionRepository(_connectionString);
                return productsProductionRepository;
            }
        }
        public RawRepository Raws
        {
            get
            {
                if (rawRepository == null)
                    rawRepository = new RawRepository(_connectionString);
                return rawRepository;
            }
        }
    }
}
