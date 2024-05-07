using FluentMigrator;
using Microsoft.Extensions.Logging;
using MinhaAgendaDeContatos.Infraestrutura.Logging;

namespace MinhaAgendaDeContatos.Infraestrutura.Migrations.Versoes;

[Migration((long)NumeroVersoes.CriarTabelaContato, "Cria tabela contato")]
public class Versao0000001 : Migration
{
    private readonly ILogger<Migration> _logger;

    public Versao0000001(ILogger<Migration> logger)
    {
        _logger = logger;
        CustomLogger.Arquivo = true;        
    }

    public override void Down()
    {
    }

    public override void Up()
    {
        CustomLogger.Arquivo = true;

        CriarTabelaContatos();
        CriarTabelaDDDRegiao();
        InserirDadosDDDRegiao();

    }

    private void CriarTabelaContatos()
    {
        var tabela = VersaoBase.InserirColunasPadrao(Create.Table("Contatos"));

        tabela
            .WithColumn("Nome").AsString(100).NotNullable()
            .WithColumn("Email").AsString().NotNullable()
            .WithColumn("Telefone").AsString(14).NotNullable()
            .WithColumn("Prefixo").AsString(14).NotNullable();

        _logger.LogInformation("Tabela 'Contatos' criada com sucesso.");
    }

    private void CriarTabelaDDDRegiao()
    {
        var tabela = VersaoBase.InserirColunasPadrao(Create.Table("DDDRegiao"));

        tabela
            .WithColumn("prefixo").AsString().NotNullable()
            .WithColumn("estado").AsString().NotNullable()
            .WithColumn("regiao").AsString().NotNullable();

        _logger.LogInformation("Tabela 'DDDRegiao' criada com sucesso.");
    }

    private void InserirDadosDDDRegiao()
    {
        // Instrução SQL para inserir múltiplos registros
        var sqlInsert = @"
        INSERT INTO public.""DDDRegiao""(""DataCriacao"",""prefixo"", ""estado"", ""regiao"") 
        VALUES 
            (CURRENT_DATE,'11','São Paulo','Região Metropolitana de São Paulo/Região Metropolitana de Jundiaí/Região Geográfica Imediata de Bragança Paulista'),
            (CURRENT_DATE,'12','São Paulo','Região Metropolitana do Vale do Paraíba e Litoral Norte'),
            (CURRENT_DATE,'13','São Paulo','Região Metropolitana da Baixada Santista/Vale do Ribeira'),
            (CURRENT_DATE,'14','São Paulo','Avaré/Bauru/Botucatu/Jaú/Lins/Marília/Ourinhos'),
            (CURRENT_DATE,'15','São Paulo','Itapetininga/Itapeva/Sorocaba/Tatuí'),
            (CURRENT_DATE,'16','São Paulo','Araraquara/Franca/Jaboticabal/Ribeirão Preto/São Carlos/Sertãozinho'),
            (CURRENT_DATE,'17','São Paulo','Barretos/Catanduva/Fernandópolis/Jales/São José do Rio Preto/Votuporanga'),
            (CURRENT_DATE,'18','São Paulo','Andradina/Araçatuba/Assis/Birigui/Dracena/Presidente Prudente'),
            (CURRENT_DATE,'19','São Paulo','Americana/Campinas/Limeira/Piracicaba/Rio Claro/São João da Boa Vista'),
            (CURRENT_DATE,'21','Rio de Janeiro','Rio de Janeiro e Região Metropolitana/Teresópolis'),
            (CURRENT_DATE,'22','Rio de Janeiro','Cabo Frio/Campos dos Goytacazes/Itaperuna/Macaé/Nova Friburgo'),
            (CURRENT_DATE,'24','Rio de Janeiro','Angra dos Reis/Petrópolis/Volta Redonda/Piraí'),
            (CURRENT_DATE,'27','Espírito Santo','Vitória e Região Metropolitana/Colatina/Linhares/Santa Maria de Jetibá'),
            (CURRENT_DATE,'28','Espírito Santo','Cachoeiro de Itapemirim/Castelo/Itapemirim/Marataízes'),
            (CURRENT_DATE,'31','Minas Gerais','Belo Horizonte e Região Metropolitana/Conselheiro Lafaiete/Ipatinga/Viçosa'),
            (CURRENT_DATE,'32','Minas Gerais','Barbacena/Juiz de Fora/Muriaé/São João del-Rei/Ubá'),
            (CURRENT_DATE,'33','Minas Gerais','Almenara/Caratinga/Governador Valadares/Manhuaçu/Teófilo Otoni'),
            (CURRENT_DATE,'34','Minas Gerais','Araguari/Araxá/Patos de Minas/Uberlândia/Uberaba'),
            (CURRENT_DATE,'35','Minas Gerais','Alfenas/Guaxupé/Lavras/Poços de Caldas/Pouso Alegre/Varginha'),
            (CURRENT_DATE,'37','Minas Gerais','Bom Despacho/Divinópolis/Formiga/Itaúna/Pará de Minas'),
            (CURRENT_DATE,'38','Minas Gerais','Curvelo/Diamantina/Montes Claros/Pirapora/Unaí'),
            (CURRENT_DATE,'41','Paraná','Curitiba e Região Metropolitana'),
            (CURRENT_DATE,'42','Paraná','Ponta Grossa/Guarapuava'),
            (CURRENT_DATE,'43','Paraná','Apucarana/Londrina'),
            (CURRENT_DATE,'44','Paraná','Maringá/Campo Mourão/Umuarama'),
            (CURRENT_DATE,'45','Paraná','Cascavel/Foz do Iguaçu'),
            (CURRENT_DATE,'46','Paraná','Francisco Beltrão/Pato Branco'),
            (CURRENT_DATE,'47','Santa Catarina','Balneário Camboriú/Blumenau/Itajaí/Joinville'),
            (CURRENT_DATE,'48','Santa Catarina','Florianópolis e Região Metropolitana/Criciúma'),
            (CURRENT_DATE,'49','Santa Catarina','Caçador/Chapecó/Lages'),
            (CURRENT_DATE,'51','Rio Grande do Sul','Porto Alegre e Região Metropolitana/Santa Cruz do Sul/Litoral Norte'),
            (CURRENT_DATE,'53','Rio Grande do Sul','Pelotas/Rio Grande'),
            (CURRENT_DATE,'54','Rio Grande do Sul','Caxias do Sul/Passo Fundo'),
            (CURRENT_DATE,'55','Rio Grande do Sul','Santa Maria/Santana do Livramento/Santo Ângelo/Uruguaiana'),
            (CURRENT_DATE,'61','Distrito Federal/Goiás','Abrangência em todo o Distrito Federal e alguns municípios da Região Integrada de Desenvolvimento do Distrito Federal e Entorno'),
            (CURRENT_DATE,'62','Goiás','Goiânia e Região Metropolitana/Anápolis/Niquelândia/Porangatu'),
            (CURRENT_DATE,'63','Tocantins','Abrangência em todo o estado'),
            (CURRENT_DATE,'64','Goiás','Caldas Novas/Catalão/Itumbiara/Rio Verde'),
            (CURRENT_DATE,'65','Mato Grosso','Cuiabá e Região Metropolitana'),
            (CURRENT_DATE,'66','Mato Grosso','Rondonópolis/Sinop'),
            (CURRENT_DATE,'67','Mato Grosso do Sul','Abrangência em todo o estado'),
            (CURRENT_DATE,'68','Acre','Abrangência em todo o estado'),
            (CURRENT_DATE,'69','Rondônia','Abrangência em todo o estado'),
            (CURRENT_DATE,'71','Bahia','Salvador e Região Metropolitana'),
            (CURRENT_DATE,'73','Bahia','Eunápolis/Ilhéus/Porto Seguro/Teixeira de Freitas'),
            (CURRENT_DATE,'74','Bahia','Irecê/Jacobina/Juazeiro/Xique-Xique'),
            (CURRENT_DATE,'75','Bahia','Alagoinhas/Feira de Santana/Paulo Afonso/Valença'),
            (CURRENT_DATE,'77','Bahia','Barreiras/Bom Jesus da Lapa/Guanambi/Vitória da Conquista'),
            (CURRENT_DATE,'79','Sergipe','Abrangência em todo o estado'),
            (CURRENT_DATE,'81','Pernambuco','Recife e Região Metropolitana/Caruaru'),
            (CURRENT_DATE,'82','Alagoas','Abrangência em todo o estado'),
            (CURRENT_DATE,'83','Paraíba','Abrangência em todo o estado'),
            (CURRENT_DATE,'84','Rio Grande do Norte','Abrangência em todo o estado'),
            (CURRENT_DATE,'85','Ceará','Fortaleza e Região Metropolitana'),
            (CURRENT_DATE,'86','Piauí','Teresina e alguns municípios da Região Integrada de Desenvolvimento da Grande Teresina/Parnaíba'),
            (CURRENT_DATE,'87','Pernambuco','Garanhuns/Petrolina/Salgueiro/Serra Talhada'),
            (CURRENT_DATE,'88','Ceará','Juazeiro do Norte/Sobral'),
            (CURRENT_DATE,'89','Piauí','Picos/Floriano'),
            (CURRENT_DATE,'91','Pará','Belém e Região Metropolitana'),
            (CURRENT_DATE,'92','Amazonas','Manaus e Região Metropolitana/Parintins'),
            (CURRENT_DATE,'93','Pará','Santarém/Altamira'),
            (CURRENT_DATE,'94','Pará','Marabá'),
            (CURRENT_DATE,'95','Roraima','Abrangência em todo o estado'),
            (CURRENT_DATE,'96','Amapá','Abrangência em todo o estado'),
            (CURRENT_DATE,'97','Amazonas','Abrangência no interior do estado'),
            (CURRENT_DATE,'98','Maranhão','São Luís e Região Metropolitana'),
            (CURRENT_DATE,'99','Maranhão','Caxias/Codó/Imperatriz');
    ";
        Execute.Sql(sqlInsert);

        _logger.LogInformation("Dados de casdastro de DDDRegiao inserido com sucesso.");
    }

}
