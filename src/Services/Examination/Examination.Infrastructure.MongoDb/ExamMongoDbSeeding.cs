using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Polly;
using Polly.Retry;
using Examination.Infrastructure.SeedWork;
using System.Collections.Generic;
using Examination.Domain.AggregateModels.CategoryAggregate;
using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Domain.AggregateModels.ExamAggregate;
using System.Linq;
using Examination.Shared.Enums;

namespace Examination.Infrastructure
{
    public class ExamMongoDbSeeding
    {
        public async Task SeedAsync(IMongoClient mongoClient, IOptions<ExamSettings> settings,
               ILogger<ExamMongoDbSeeding> logger)
        {
            var policy = CreatePolicy(logger, nameof(ExamMongoDbSeeding));
            await policy.ExecuteAsync(async () =>
            {
                var databaseName = settings.Value.DatabaseSettings.DatabaseName;
                var database = mongoClient.GetDatabase(databaseName);
                var categoryId1 = ObjectId.GenerateNewId().ToString();
                var categoryId2 = ObjectId.GenerateNewId().ToString();
                var categoryId3 = ObjectId.GenerateNewId().ToString();
                var categoryId4 = ObjectId.GenerateNewId().ToString();
                var categoryId5 = ObjectId.GenerateNewId().ToString();
                if (await database.GetCollection<Category>(Constants.Collections.Category).EstimatedDocumentCountAsync() == 0)
                {
                    await database.GetCollection<Category>(Constants.Collections.Category)
                        .InsertManyAsync(new List<Category>()
                        {
                            new Category(categoryId1,"Tin học 12","tin-hoc-12"),
                            new Category(categoryId2,"Tin học 11","tin-hoc-11"),
                            new Category(categoryId3,"Tin học 10","tin-hoc-10"),
                            new Category(categoryId4,"Tin học 7","tin-hoc-7"),
                            new Category(categoryId5,"Tin học 6","tin-hoc-6")
                        });
                }
                if (await database.GetCollection<Question>(Constants.Collections.Question).EstimatedDocumentCountAsync() ==
                    0)
                {
                    await database.GetCollection<Question>(Constants.Collections.Question)
                        .InsertManyAsync(GetPredefinedQuestions(categoryId2));
                }
                if (await database.GetCollection<Exam>(Constants.Collections.Exam).EstimatedDocumentCountAsync() ==
                    0)
                {
                    await database.GetCollection<Exam>(Constants.Collections.Exam)
                        .InsertManyAsync(GetPredefinedExams(categoryId2));
                }
            });
        }

        private List<Exam> GetPredefinedExams(string categoryId)
        {
            return new List<Exam>()
            {
                new Exam("Tin học 11 - Đề 1", "Câu hỏi ôn tập Tin học 11 - Đề 1",
                    "<p>Nội dung ôn tập: Chủ đề 1 & 2</p>",
                    10,
                    "10:00",
                    GetPredefinedQuestions(categoryId).Take(10).ToList(),
                    Level.Easy,
                    false,
                    null,
                    6,
                    true, null, null),
                new Exam("Tin học 11 - Đề 2", "Câu hỏi ôn tập Tin học 11 - Đề 2",
                    "<p>Ôn tập chuẩn bị cho bài kiểm tra giữa kỳ 1.</p>",
                    5,
                    "05:00",
                    GetPredefinedQuestions(categoryId).Skip(5).Take(5).ToList(),
                    Level.Medium,
                    true,
                    null,
                    3,
                    true, null, null),

               //new Exam("Exam 2", "Lorem Ipsum is simply dummy text of the printing and typesetting industry",
               //     "<p>Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.</p>",
               //     5,
               //     "05:00",
               //     GetPredefinedQuestions(categoryId).Skip(5).Take(5).ToList(),
               //     Level.Medium,
               //     null,
               //     4,
               //     true, null, null),
            };
        }
        private List<Question> GetPredefinedQuestions(string categoryId)
        {
            return new List<Question>()
            {
                new("608cd754ef63d3914679ea5b","Điền vào chỗ trống: “Chương trình viết bằng … có thể được nạp trực tiếp vào bộ nhớ và thực hiện ngay”.", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new("608df4a7fe65c8efb4470544","ngôn ngữ bậc cao."),
                        new("608df5376a9d681574657cc2","ngôn ngữ máy.", true),
                        new("608df53aa5f60ba58550133f","hợp ngữ và ngôn ngữ bậc cao."),
                        new("608df53de99c4060eb629fe8","hợp ngữ.")
                    },
                    "Tin 11 - Gợi ý giải"),
                new("608cd754ef63d3914679ea60","Chương trình đặc biệt có chức năng chuyển đổi chương trình được viết bằng ngôn ngữ lập trình bậc cao thành chương trình thực hiện được trong máy tính được gọi là:", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new("608df543c68c39b8f35a5045","thông dịch."),
                        new("608df54665f59b22462d175c","chương trình dịch.", true),
                        new("608df54be0f049cbeaba337d","ngôn ngữ lập trình."),
                        new("608df54e0fecd3136940353c","biên dịch.")
                    },
                    "Tin 11 - Gợi ý giải"),
                new("608cd754ef63d3914679ea65","Tên là một dãy liên tiếp không quá 256 kí tự bao gồm chữ số, chữ cái hoặc dấu gạch dưới và bắt đầu bằng?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new("608df55272c30e7832703226","chữ số."),
                        new("608df556bd2b451e6b2f40db","ký tự đặc biệt."),
                        new("608df5598fb7fddcaff2c2b8","chữ số hoặc dấu gạch dưới."),
                        new("608df55cb735565b0846e9d0","chữ cái hoặc dấu gạch dưới.", true)
                    },
                    "Tin 11 - Gợi ý giải"),
                new("608cd754ef63d3914679ea6a","Chọn đáp án sai?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new("608df560ac4ca31f8cd6af13","Hằng là đại lượng có giá trị không thay đổi trong quá trình thực hiện chương trình."),
                        new("608df564e6d559052dd221db","Biến là đại lượng được đặt tên, có giá trị không thay đổi trong quá trình thực hiện chương trình.", true),
                        new("608df5681eee6ec535381ade","Tên do người lập trình đặt cần theo nguyên tắc đặt tên của mỗi ngôn ngữ lập trình."),
                        new("608df56bb74fea686a852a51","Tên dành riêng không thể được dùng để đặt tên biến trong chương trình.")
                    },
                    "Tin 11 - Gợi ý giải"),
                new("608cd754ef63d3914679ea6f","Trường hợp nào dưới đây là tên biến trong Python?", QuestionType.SingleSelection, Level.Medium, categoryId,
                    new List<Answer>()
                    {
                        new("608df570521ffdebd9aa0e57","bai-tap-1"),
                        new("608df574690bbf32f92dc286","baiTap1", true),
                        new("608df5779027cfe87b85fef7","_bai tap 1"),
                        new("608df57dd002589bd1d68ea9","bai tap 1")
                    },
                    "Tin 11 - Gợi ý giải"),
                new("608cd754ef63d3914679ea74","Trong Python, cho chương trình (4 dòng):\nx = 25 \t\t\t# Dòng 1\ny = 18, z = -32 \t# Dòng 2\na = 5; b = 15 \t\t# Dòng 3\nc, d = 10, 20 \t\t# Dòng 4\nKhi thực hiện, chương trình trên báo lỗi sai tại dòng?\n", QuestionType.SingleSelection, Level.Medium, categoryId,
                    new List<Answer>()
                    {
                        new("608df585790fb419be6bb1d4","Dòng 1."),
                        new("608df58b6c77aedda3ebc00f","Dòng 2.", true),
                        new("608df58e1d30025e41addcc4","Dòng 3."),
                        new("608df5927bc56f38d4101385","Dòng 4.")
                    },
                    "Tin 11 - Gợi ý giải"),
                new("608cd754ef63d3914679ea79","Trong Python, ký hiệu ‘int’ là dữ liệu kiểu:", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new("608df59789452139c8f92c75","nguyên.", true),
                        new("608df59ad8cb53e423462e36","thực."),
                        new("608df59ce3a7c34d19a8a440","xâu."),
                        new("608df59fee7260bd17889e28","lôgic.")
                    },
                    "Tin 11 - Gợi ý giải"),
                new("608cd754ef63d3914679ea7e","Trong Python, để chuyển đổi kiểu dữ liệu sử dụng cú pháp?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new("608df5a476d72c977b4fe1ac","<giá trị>(<tên kiểu dữ liệu>)"),
                        new("608df5a875887b4fecd2445b","<tên kiểu dữ liệu><giá trị>"),
                        new("608df5ab6acaaa22696b6b24","<tên kiểu dữ liệu>[<giá trị>]"),
                        new("608df5af6a192b13f317f9ee","<tên kiểu dữ liệu>(<giá trị>)", true)
                    },
                    "Tin 11 - Gợi ý giải"),
                new("608cd754ef63d3914679ea83","Trong Python, cú pháp: type(<tên biến>) sẽ cho biết?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new("608df5b3cf787a7520d0965e","tên biến."),
                        new("608df5b73e6d19ad3a033670","giá trị của biến."),
                        new("608df5bb471294f71dd4818c","kiểu dữ liệu của biến.", true),
                        new("608df5bec65ffde211cd31c5","danh sách biến.")
                    },
                    "Tin 11 - Gợi ý giải"),
                new("608cd754ef63d3914679ea88","Trong Python, kết quả của biểu thức: (5*2+10) là dữ liệu kiểu?", QuestionType.SingleSelection, Level.Medium, categoryId,
                    new List<Answer>()
                    {
                        new("608df5c1ec2dee54bc500cf3","int.", true),
                        new("608df5c419a81be17ade4f5d","float."),
                        new("608df5c860d965e5e25584c5","str."),
                        new("608df5cc5fc1779a5da58532","bool.")
                    },                   
                    "Tin 11 - Gợi ý giải"),
               new(ObjectId.GenerateNewId().ToString(),"Trong Python, khai báo biến nào sau đây không đúng?", QuestionType.SingleSelection, Level.Medium, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"n = 50"),
                        new(ObjectId.GenerateNewId().ToString(),"f = 17.5"),
                        new(ObjectId.GenerateNewId().ToString(),"s = ‘Bac Ai’"),
                        new(ObjectId.GenerateNewId().ToString(),"2b = True", true)
                    },
                    "Tin 11 - Gợi ý giải"),
                new(ObjectId.GenerateNewId().ToString(),"Trong Python, kết quả xuất ra màn hình của câu lệnh: print(5+4*3-7*4) là??", QuestionType.SingleSelection, Level.Difficult, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"80"),
                        new(ObjectId.GenerateNewId().ToString(),"11"),
                        new(ObjectId.GenerateNewId().ToString(),"-11", true),
                        new(ObjectId.GenerateNewId().ToString(),"-144")
                    },
                    "Tin 11 - Gợi ý giải"),
                new(ObjectId.GenerateNewId().ToString(),"Trong Python, kết quả xuất ra màn hình của chương trình (4 dòng) sau là gì?\r\na = 'THCS, THPT'\r\nb = 'Bac Ai'\r\nc = b + a\r\nprint(c)\r\n", QuestionType.SingleSelection, Level.Hardest, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"THCS, THPTBac Ai"),
                        new(ObjectId.GenerateNewId().ToString(),"THCS, THPT Bac Ai"),
                        new(ObjectId.GenerateNewId().ToString(),"Bac Ai THCS, THPT"),
                        new(ObjectId.GenerateNewId().ToString(),"Bac AiTHCS, THPT", true)
                    },
                    "Tin 11 - Gợi ý giải")
            };
        }
        private AsyncRetryPolicy CreatePolicy(ILogger<ExamMongoDbSeeding> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<MongoException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
                    }
                );
        }

        //new("608cd754ef63d3914679ea5b","Question 1", QuestionType.SingleSelection, Level.Easy, categoryId1,
        //    new List<Answer>()
        //    {
        //        new("608df4a7fe65c8efb4470544","Answer 1", true),
        //        new("608df5376a9d681574657cc2","Answer 2"),
        //        new("608df53aa5f60ba58550133f","Answer 3"),
        //        new("608df53de99c4060eb629fe8","Answer 3")
        //    },
        //    "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit..."),
        //new("608cd754ef63d3914679ea60","Question 2", QuestionType.SingleSelection, Level.Easy, categoryId1,
        //    new List<Answer>()
        //    {
        //        new("608df543c68c39b8f35a5045","Answer 1"),
        //        new("608df54665f59b22462d175c","Answer 2", true),
        //        new("608df54be0f049cbeaba337d","Answer 3"),
        //        new("608df54e0fecd3136940353c","Answer 3")
        //    },
        //    "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit..."),
        //new("608cd754ef63d3914679ea65","Question 3", QuestionType.SingleSelection, Level.Easy, categoryId1,
        //    new List<Answer>()
        //    {
        //        new("608df55272c30e7832703226","Answer 1", true),
        //        new("608df556bd2b451e6b2f40db","Answer 2"),
        //        new("608df5598fb7fddcaff2c2b8","Answer 3"),
        //        new("608df55cb735565b0846e9d0","Answer 3")
        //    },
        //    "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit..."),
        //new("608cd754ef63d3914679ea6a","Question 4", QuestionType.SingleSelection, Level.Easy, categoryId1,
        //    new List<Answer>()
        //    {
        //        new("608df560ac4ca31f8cd6af13","Answer 1"),
        //        new("608df564e6d559052dd221db","Answer 2", true),
        //        new("608df5681eee6ec535381ade","Answer 3"),
        //        new("608df56bb74fea686a852a51","Answer 3")
        //    },
        //    "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit..."),
        //new("608cd754ef63d3914679ea6f","Question 5", QuestionType.SingleSelection, Level.Easy, categoryId1,
        //    new List<Answer>()
        //    {
        //        new("608df570521ffdebd9aa0e57","Answer 1", true),
        //        new("608df574690bbf32f92dc286","Answer 2"),
        //        new("608df5779027cfe87b85fef7","Answer 3"),
        //        new("608df57dd002589bd1d68ea9","Answer 3")
        //    },
        //    "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit..."),
        //new("608cd754ef63d3914679ea74","Question 6", QuestionType.SingleSelection, Level.Easy, categoryId1,
        //    new List<Answer>()
        //    {
        //        new("608df585790fb419be6bb1d4","Answer 1"),
        //        new("608df58b6c77aedda3ebc00f","Answer 2", true),
        //        new("608df58e1d30025e41addcc4","Answer 3"),
        //        new("608df5927bc56f38d4101385","Answer 3")
        //    },
        //    "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit..."),
        //new("608cd754ef63d3914679ea79","Question 7", QuestionType.SingleSelection, Level.Easy, categoryId1,
        //    new List<Answer>()
        //    {
        //        new("608df59789452139c8f92c75","Answer 1"),
        //        new("608df59ad8cb53e423462e36","Answer 2"),
        //        new("608df59ce3a7c34d19a8a440","Answer 3", true),
        //        new("608df59fee7260bd17889e28","Answer 3")
        //    },
        //    "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit..."),
        //new("608cd754ef63d3914679ea7e","Question 8", QuestionType.SingleSelection, Level.Easy, categoryId1,
        //    new List<Answer>()
        //    {
        //        new("608df5a476d72c977b4fe1ac","Answer 1"),
        //        new("608df5a875887b4fecd2445b","Answer 2"),
        //        new("608df5ab6acaaa22696b6b24","Answer 3"),
        //        new("608df5af6a192b13f317f9ee","Answer 3", true)
        //    },
        //    "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit..."),
        //new("608cd754ef63d3914679ea83","Question 9", QuestionType.SingleSelection, Level.Easy, categoryId1,
        //    new List<Answer>()
        //    {
        //        new("608df5b3cf787a7520d0965e","Answer 1"),
        //        new("608df5b73e6d19ad3a033670","Answer 2"),
        //        new("608df5bb471294f71dd4818c","Answer 3", true),
        //        new("608df5bec65ffde211cd31c5","Answer 3")
        //    },
        //    "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit..."),
        //new("608cd754ef63d3914679ea88","Question 10", QuestionType.SingleSelection, Level.Easy, categoryId1,
        //    new List<Answer>()
        //    {
        //        new("608df5c1ec2dee54bc500cf3","Answer 1", true),
        //        new("608df5c419a81be17ade4f5d","Answer 2"),
        //        new("608df5c860d965e5e25584c5","Answer 3"),
        //        new("608df5cc5fc1779a5da58532","Answer 3")
        //    },
        //    "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit..."),

    }
}