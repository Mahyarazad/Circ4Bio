namespace _0_Framework.Application.Email
{
    public class EmailBuilder
    {
        public static string Builder(EmailBody Command)
        {
            var template = "<!DOCTYPE html>                                                                 " +
                           "<html>                                                                        " +
                           "<head>                                                                        " +
                           "    <link href='https://fonts.googleapis.com/css2?family=Glory:wght@100&display=swap'" +
                           "          rel='stylesheet'>                                                   " +
                           "    <style>                                                                   " +
                           "        .container {                                                          " +
                           "            font-family: 'Glory', sans-serif;                                 " +
                           "            height: 100%;                                                     " +
                           "            display: grid;                                                    " +
                           "            grid-template-columns: 0.4fr;                                     " +
                           "            grid-template-rows: 0.5fr 0.35fr 1fr;                             " +
                           "            grid-template-areas:                                              " +
                           "                'header'                                                      " +
                           "                'order-container'                                             " +
                           "                'table-container';                                            " +
                           "        }                                                                     " +
                           "                                                                              " +
                           "        .header {                                                             " +
                           "            grid-area: header;                                                " +
                           "            grid-row-start: 1;                                                " +
                           "            grid-column: 1;                                                   " +
                           "            place-self: center;                                               " +
                           "        }                                                                     " +
                           "                                                                              " +
                           "        .order-container {                                                    " +
                           "            grid-area: orderDetail;                                           " +
                           "            grid-row-start: 2;                                                " +
                           "            grid-column: 1;                                                   " +
                           "            align-self: start;                                                " +
                           "        }                                                                     " +
                           "                                                                              " +
                           "        .table-container {                                                    " +
                           "            grid-area: table-container;                                       " +
                           "            grid-row-start: 3;                                                " +
                           "            grid-column: 1;                                                   " +
                           "            overflow: hidden;                                                 " +
                           "            align-self: start;                                                " +
                           "        }                                                                     " +
                           "                                                                              " +
                           "        .image {                                                              " +
                           "            height:200px;                                                            " +
                           "            width:200px;                                                            " +
                           "            vertical-align:middle;                                                            " +
                           "            vertical-align:middle;                                                            " +
                           "        }                                                                     " +
                           "                                                                              " +
                           "        p.bigger {                                                            " +
                           "            margin-bottom: 0;                                                 " +
                           "            font-size: 3.5em;                                                 " +
                           "            text-align: center;                                               " +
                           "        }                                                                     " +
                           "                                                                              " +
                           "        p.smaller {                                                           " +
                           "            text-align: center;                                               " +
                           "            font-size: 2em;                                                   " +
                           "        }                                                                     " +
                           "                                                                              " +
                           "        p.text {                                                              " +
                           "            font-size: 1.5em;                                                 " +
                           "            text-align: center;                                               " +
                           "        }                                                                     " +
                           "                                                                              " +
                           "        table {                                                               " +
                           "            width: 100%;                                                      " +
                           "            font-size: 1em;                                                   " +
                           "            border: 0.1em solid black;                                        " +
                           "            border-radius: 0.2em;                                             " +
                           "        }                                                                     " +
                           "                                                                              " +
                           "        tr {                                                                  " +
                           "            text-align: center;                                               " +
                           "            border: 0.1em solid black;                                        " +
                           "        }                                                                     " +
                           "                                                                              " +
                           "        th {                                                                  " +
                           "            font-size: 1em;                                                   " +
                           "        }                                                                     " +
                           "    </style>                                                                  " +
                           "</head>                                                                       " +
                           "<body>                                                                        " +
                           "    <div class='container'>                                                   " +
                           "        <div class='header'>                                                  " +
                           $"            <p class='bigger'> {Command.Title} </p>                  " +
                           $"            <p class='smaller'> {Command.ShortDescription}</p>           " +
                           "            <img class='image'                                                " +
                           "                 src='http://www.maahyarazad.ir/Images/Circ4Bio.png'                                                       " +
                           "                 alt='featured-thumbnail' />                                  " +
                           "        </div>                                                                " +
                           "        <div class='order-container'>                                         " +
                           "            <p class='smaller' style='margin-bottom: 1em;'>                      " +
                           "                <b>                                                           " +
                           $"                    {Command.Description}                                          " +
                           "                </b>                           " +
                           "            </p>                                                              " +
                           "                                                                              " +
                           "        </div>                                                                " +
                           "    </div>                                                                    " +
                           "</body>                                                                       " +
                           "</html>                                                                       ";
            return template;
        }
    }

    public class EmailBody
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
    }
}