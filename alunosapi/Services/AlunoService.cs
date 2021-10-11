using AlunosApi.Context;
using AlunosApi.Models;
using AlunosApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlunosApi.Services
{
    public class AlunoService : IAlunoService
    {
        private readonly AppDbContext _context;

        public AlunoService(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<IEnumerable<Aluno>> GetAlunos()
        {
            try
            {
                // AsNoTracking(): há ganho de desempenho por não rastrear as entidades, porém não terá possibilidades de alterações como updates (seria criado um registro novo)
                return await _context.Alunos.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Aluno> GetAluno(int id)
        {
            try
            {
                return await _context.Alunos.FindAsync(id);
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<Aluno>> GetAlunosByNome(string nome)
        {
            try
            {
                return (String.IsNullOrEmpty(nome))
                    ? await this.GetAlunos()
                    : await _context.Alunos.Where(x => x.Nome.Contains(nome)).ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task CreateAluno(Aluno aluno)
        {
            _context.Alunos.Add(aluno);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAluno(Aluno aluno)
        {
            _context.Entry(aluno).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAluno(Aluno aluno)
        {
            _context.Alunos.Remove(aluno);
            await _context.SaveChangesAsync();
        }
    }
}
