using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using keepr.Models;
using Microsoft.AspNetCore.Mvc;

namespace keepr.Repositories
{
  public class VaultsRepository
  {
    private readonly IDbConnection _db;
    public VaultsRepository(IDbConnection db)
    {
      _db = db;
    }

    // SECTION CRUD methods
    public IEnumerable<Vault> GetVaults()
    {
      return _db.Query<Vault>("SELECT * FROM vaults");
    }

    public Vault GetVaultById(int id)
    {
      return _db.QueryFirstOrDefault<Vault>("SELECT * FROM vaults WHERE id = @id", new { id });
    }
    public Vault CreateVault(Vault vault)
    {
      var id = _db.ExecuteScalar<int>(@"
      INSERT INTO vaults (name, description)
      VALUES (@Name, @Description);
      SELECT LAST_INSERT_ID();
      ", vault);
      vault.Id = id;
      return vault;
    }
    public void DeleteVault(int id)
    {
      var success = _db.Execute("DELETE FROM vaults WHERE id = @id", new { id });
      if (success != 1)
      {
        throw new Exception("Delete failed");
      }
    }

  }
}