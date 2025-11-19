using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SmartStockClienteWinForms.Helpers
{
    public static class Temas
    {
        public static Color FondoPrincipal => ColorTranslator.FromHtml("#FEFCFB");
        public static Color PanelLateral => ColorTranslator.FromHtml("#0A1128");
        public static Color BarraSuperior => ColorTranslator.FromHtml("#001F54");
        public static Color BotonPrimario => ColorTranslator.FromHtml("#034078");
        public static Color BotonSecundario => ColorTranslator.FromHtml("#1282A2");
        public static Color TextoClaro => ColorTranslator.FromHtml("#FEFCFB");
        public static Color TextoOscuro => ColorTranslator.FromHtml("#0A1128");
    }
}