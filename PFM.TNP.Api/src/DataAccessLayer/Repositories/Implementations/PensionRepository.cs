using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interfaces;

namespace DataAccessLayer.Repositories.Implementations;

public class PensionRepository() : BaseRepository<Pension>(), IPensionRepository;