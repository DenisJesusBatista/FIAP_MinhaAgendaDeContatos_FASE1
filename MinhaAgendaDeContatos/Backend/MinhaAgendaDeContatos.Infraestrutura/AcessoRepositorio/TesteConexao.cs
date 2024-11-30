using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinhaAgendaDeContatos.Infraestrutura.AcessoRepositorio
{
    public class TesteConexao
    {
        private readonly MinhaAgendaDeContatosContext _context;

        public TesteConexao(MinhaAgendaDeContatosContext context)
        {
            _context = context;
        }

        public async Task TestarConexao()
        {
            try
            {
                // Apenas um teste básico para verificar se o contexto pode se conectar ao banco de dados
                var canConnect = await _context.Database.CanConnectAsync();
                Console.WriteLine($"Conexão bem-sucedida: {canConnect}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao conectar: {ex.Message}");
            }
        }
    }

}
