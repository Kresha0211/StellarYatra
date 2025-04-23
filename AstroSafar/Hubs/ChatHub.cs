using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace AstroSafar.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task GetResponse(string message)
        {
            string reply;
            message = message.ToLower();

            // Greetings
            if (message.Contains("hello") || message.Contains("hi") || message.Contains("hey"))
                reply = "Hello there! 👋 How can I assist you today?";

            // Course information
            else if (message.Contains("course") || message.Contains("class") || message.Contains("program"))
                reply = "We offer various courses in astronomy and space science. You can browse them under Primary, Secondary, and Higher Secondary categories. Would you like to know about a specific course?";

            // Certificates
            else if (message.Contains("certificate") || message.Contains("certification"))
                reply = "Certificates are awarded after course completion and payment🎓.";

            // Enrollment
            else if (message.Contains("enroll") || message.Contains("join") || message.Contains("register"))
                reply = "To enroll:  1) Create an account 2) Select your course 3) Get Enrolled  4) Start learning! Need help with any step?";

            // Tests/Quizzes
            else if (message.Contains("test") || message.Contains("quiz") || message.Contains("exam"))
                reply = "Quizzes are available after completing each module. Final exams unlock after finishing all unit content.";

            // Payments
            else if (message.Contains("payment") || message.Contains("pay") || message.Contains("fee"))
                reply = "We accept: 💳 Credit/Debit Cards,UPI and Bank Transfers. Payment is required before accessing Certificate.";

            // Book store information
            else if (message.Contains("book") || message.Contains("buy") || message.Contains("purchase") ||
                     message.Contains("store") || message.Contains("shop"))
                reply = "You can explore and buy astronomy books from our store. Here's how:\n" +
                        "1) Visit the 'Books' section\n" +
                        "2) Browse our collection\n" +
                        "3) Click 'Explore Book' to learn more\n" +
                        "4) Add to cart when ready\n" +
                        "Need help finding a specific book?";

            // Book delivery/shipping
            else if (message.Contains("delivery") || message.Contains("shipping") || message.Contains("dispatch"))
                reply = "📦 Book delivery info:\n" +
                        "- Standard shipping: 5-7 business days\n" +
                        "- Express shipping: 2-3 business days (additional fee)\n" +
                        "- We ship worldwide!";

            // Book prices
            else if (message.Contains("price") || message.Contains("cost") || message.Contains("how much"))
                reply = "Our book prices vary by title and format:\n" +
                        "- Physical books: ₹500-₹2000\n" +
                        "- E-books: 50% of physical book price\n" +
                        "- Bundles available at discounted rates";

            // Book recommendations
            else if (message.Contains("recommend") || message.Contains("suggest") || message.Contains("best book"))
                reply = "Popular recommendations:\n" +
                        "1. 'Cosmos' by Carl Sagan\n" +
                        "2. 'Astrophysics for People in a Hurry'\n" +
                        "3. 'The Martian' by Andy Weir\n" +
                        "Would you like details on any of these?";

            // Guidelines
            else if (message.Contains("guide") || message.Contains("instruction") || message.Contains("how to"))
                reply = "Here are key guidelines:\n1) Complete modules in order\n2) Take notes during videos\n3) Attempt quizzes seriously";

            // Technical help
            else if (message.Contains("technical") || message.Contains("problem") || message.Contains("issue"))
                reply = "For technical issues:\n1) Try refreshing\n2) Clear browser cache\n3) Check your internet\nStill stuck? Email tech@astrosafar.com with screenshots.";

            // Contact information
            else if (message.Contains("contact") || message.Contains("email") || message.Contains("call"))
                reply = "Reach us at:\n📧 support@astrosafar.com\n📞 +1 (555) 123-4567 (10AM-6PM UTC)";

            // About us
            else if (message.Contains("about") || message.Contains("who are you"))
                reply = "AstroSafar is a premier astronomy education platform founded in 2015. We've taught over 50,000 students with a 4.9/5 satisfaction rating!";

            // Default response
            else
                reply = "Sorry, I couldn't understand that. Can you rephrase?";

            // Send response only to the caller (not all clients)
            await Clients.Caller.SendAsync("ReceiveResponse", reply);
        }
    }
}