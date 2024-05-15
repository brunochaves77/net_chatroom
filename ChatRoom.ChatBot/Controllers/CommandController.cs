using ChatRoom.Application.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatRoom.ChatBot.Controllers {
    public static class CommandController {

        private const int PosicaoNomeCSV = 0;
        private const int PosicaoValorFechamento = 6;

        public static async Task<string> StockCommand(RabbitMessageDTO message) {

            var code = message.Message.Replace("/stock=", "");
            var url = $"https://stooq.com/q/l/?s={code}.us&f=sd2t2ohlcv&h&e=csv";

            var client = new HttpClient();
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode) {

                using (var theStream = await response.Content.ReadAsStreamAsync())
                using (var theStreamReader = new StreamReader(theStream)) {

                    string? theLine = null;

                    var line = 0;

                    while ((theLine = await theStreamReader.ReadLineAsync()) != null) {

                        line++;

                        if (line == 1)
                            continue;

                        var csv = theLine.Split(',');

                        var nome = csv[PosicaoNomeCSV];
                        var valor = csv[PosicaoValorFechamento];

                        if (valor == "N/D")
                            return $"Code '{code}' not founded";

                        var stringFinal = $"{nome.ToUpper()} quote is ${valor} per share";
                        return stringFinal;

                    }
                };

            } else {
                return $"Error ({response.StatusCode})";
            }

            return $"Error";

        }

    }
}
