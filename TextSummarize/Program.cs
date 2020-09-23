using Serilog;
using System;
using TextRank;

namespace TextSummarize
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();
                        
            var sentence =
                "LINCOLNSHIRE, IL With next-generation video game systems such as the Xbox One and the Playstation 4 hitting stores later this month, the console wars got even hotter today as electronics manufacturer Zenith announced the release of its own console, the Gamespace Pro, which arrives in stores Nov. 19. “With its sleek silver-and-gray box, double-analog-stick controllers, ability to play CDs, and starting price of $374.99, the Gamespace Pro is our way of saying, ‘Move over, Sony and Microsoft, Zenith is now officially a player in the console game,’” said Zenith CEO Michael Ahn at a Gamespace Pro press event, showcasing the system’s launch titles MoonChaser: Radiation, Cris Collinsworth’s Pigskin 2013, and survival-horror thriller InZomnia. “With over nine launch titles, 3D graphics, and the ability to log on to the internet using our Z-Connect technology, Zenith is finally poised to make some big waves in the video game world.” According to Zenith representatives, over 650 units have already been preordered.";
            var sentence1 =
                @"World's largest retailer Walmart today said its USD 16 billion investment in Flipkart may negatively impact its earnings per share (EPS) by USD 0.25-0.30 in fiscal year 2019, even though it remains ""excited about what the future holds"" in the fast growing Indian e-commerce space. ""The company's investment in Flipkart... is expected to negatively impact fiscal year 2019 EPS by approximately USD 0.25 to USD 0.30 if the transaction closes at the end of the second quarter,"" Walmart said in its earnings statement.
                The US-based company has reported better-than-expected revenue numbers in the first quarter with total revenue growing 4.4 per cent to USD 122.7 billion. Its online sales grew 33 per cent in the said quarter and expects it to grow about 40 per cent for the full year.
                Walmart's net sales from international business grew 11.7 per cent to USD 30.3 billion in the said quarter. The diluted EPS stood at USD 0.72, while the adjusted EPS was at USD 1.14, the statement added.
                The statement also noted the steps taken by the company during the quarter to reposition its portfolio of businesses. These include Walmart's investment in Flipkart, combining Sainsbury and Asda in the UK and divestment of banking operations in Walmart Canada and Walmart Chile.
                On the deal with Flipkart in India, Walmart President and CEO Doug McMillon said e-commerce in India is growing rapidly and it expects the segment to grow at four times the rate of overall retail.
                ""Flipkart is already capturing a large portion of this growth and is well-positioned to accelerate into the future. So, this is an investment in a large, fast-growing country, with an innovative business positioned in the growth area of e-commerce... We're excited about what the future holds,"" he added.
                After months of discussions, Walmart had announced the mega deal to pick up 77 per cent stake in Flipkart Group holding company that is registered in Singapore. The transaction is subject to statutory approvals, including that of Competition Commission of India (CCI).
                The deal, which valued the Bengaluru-based company at USD 20.8 billion, is believed to be part of Walmart's strategy to strengthen presence in the Indian market and also compete head-on with global rival, Amazon.
                Amazon is also a rival to Flipkart and the two are locked in an intense battle for leadership in the booming Indian e-commerce market that is forecast to touch USD 200 billion in the next few years.";

            var phrases = sentence1.KeyPhrases();

            Log.Information("Extracted Phrases : {Phrases}", phrases.Item1);
            Log.Information(sentence1);

            Console.ReadLine();
        }
    }
}
