using Examination.Domain.AggregateModels.CategoryAggregate;
using Examination.Domain.AggregateModels.ExamAggregate;
using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Infrastructure.SeedWork;
using Examination.Shared.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                var categoryId6 = ObjectId.GenerateNewId().ToString();
                var categoryId7 = ObjectId.GenerateNewId().ToString();
                var categoryId8 = ObjectId.GenerateNewId().ToString();
                var categoryId9 = ObjectId.GenerateNewId().ToString();
                if (await database.GetCollection<Category>(Constants.Collections.Category).EstimatedDocumentCountAsync() == 0)
                {
                    await database.GetCollection<Category>(Constants.Collections.Category)
                        .InsertManyAsync(new List<Category>()
                        {
                            new Category(categoryId1,"Tin học 12","tin-hoc-12"),
                            new Category(categoryId2,"Tin học 11","tin-hoc-11"),
                            new Category(categoryId3,"Tin học 10","tin-hoc-10"),
                            new Category(categoryId4,"Tin học 7","tin-hoc-7"),
                            new Category(categoryId5,"Tin học 6","tin-hoc-6"),
                            new Category(categoryId6,"Quốc phòng 10","quoc-phong-10"),
                            new Category(categoryId7,"Sinh học 9","sinh-hoc-9"),
                            new Category(categoryId8,"Vật lý 7","vat-ly-7"),
                            new Category(categoryId9,"Hóa học 8","hoa-hoc-8"),
                        });
                }
                if (await database.GetCollection<Question>(Constants.Collections.Question).EstimatedDocumentCountAsync() == 0)
                {
                    await database.GetCollection<Question>(Constants.Collections.Question).InsertManyAsync(GetPredefinedQuestions(categoryId2));

                    await database.GetCollection<Question>(Constants.Collections.Question).InsertManyAsync(GetPredefinedQuestions_QP10(categoryId6));
                    await database.GetCollection<Question>(Constants.Collections.Question).InsertManyAsync(GetPredefinedQuestions_SinhHoc9(categoryId7));
                    await database.GetCollection<Question>(Constants.Collections.Question).InsertManyAsync(GetPredefinedQuestions_VatLy7(categoryId8));
                    await database.GetCollection<Question>(Constants.Collections.Question).InsertManyAsync(GetPredefinedQuestions_TinHoc12(categoryId1));
                }
                if (await database.GetCollection<Exam>(Constants.Collections.Exam).EstimatedDocumentCountAsync() == 0)
                {
                    await database.GetCollection<Exam>(Constants.Collections.Exam).InsertManyAsync(GetPredefinedExams(categoryId2));

                    await database.GetCollection<Exam>(Constants.Collections.Exam).InsertManyAsync(GetPredefinedExams_QP10(categoryId6));
                    await database.GetCollection<Exam>(Constants.Collections.Exam).InsertManyAsync(GetPredefinedExams_SinhHoc9(categoryId7));
                    await database.GetCollection<Exam>(Constants.Collections.Exam).InsertManyAsync(GetPredefinedExams_VatLy7(categoryId8));
                    await database.GetCollection<Exam>(Constants.Collections.Exam).InsertManyAsync(GetPredefinedExams_TinHoc12(categoryId1));
                }

                // THÊM QP10

            });
        }

        private List<Exam> GetPredefinedExams(string categoryId)
        {
            return new List<Exam>()
            {
                // Tạo đề 1 - Tin 11
                new Exam("Tin học 11 - Chủ đề 1 & 2", "Năm học: 2022-2023",
                    "Nội dung ôn tập: Chủ đề 1 & 2",
                    10,
                    "10:00",
                    GetPredefinedQuestions(categoryId).Take(10).ToList(),
                    Level.Easy,
                    false,
                    null,
                    6,
                    true, null, null),

                // Tạo đề 2 - Tin 11
                new Exam("Tin học 11 - Luyện tâp nhanh", "Năm học: 2022-2023",
                    "Ôn tập kiểm tra giữa kỳ 1",
                    5,
                    "05:00",
                    GetPredefinedQuestions(categoryId).Skip(5).Take(5).ToList(),
                    Level.Medium,
                    false,
                    null,
                    3,
                    true, null, null)
            };
        }
        private List<Question> GetPredefinedQuestions(string categoryId)
        {
            return new List<Question>()
            {
                // DANH SÁCH CÂU HỎI TIN 11
                new(ObjectId.GenerateNewId().ToString(),"Điền vào chỗ trống: “Chương trình viết bằng … có thể được nạp trực tiếp vào bộ nhớ và thực hiện ngay”.", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"ngôn ngữ bậc cao."),
                        new(ObjectId.GenerateNewId().ToString(),"ngôn ngữ máy.", true),
                        new(ObjectId.GenerateNewId().ToString(),"hợp ngữ và ngôn ngữ bậc cao."),
                        new(ObjectId.GenerateNewId().ToString(),"hợp ngữ.")
                    },
                    "Tin 11 - Gợi ý giải"),
                new(ObjectId.GenerateNewId().ToString(),"Chương trình đặc biệt có chức năng chuyển đổi chương trình được viết bằng ngôn ngữ lập trình bậc cao thành chương trình thực hiện được trong máy tính được gọi là:", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"thông dịch."),
                        new(ObjectId.GenerateNewId().ToString(),"chương trình dịch.", true),
                        new(ObjectId.GenerateNewId().ToString(),"ngôn ngữ lập trình."),
                        new(ObjectId.GenerateNewId().ToString(),"biên dịch.")
                    },
                    "Tin 11 - Gợi ý giải"),
                new(ObjectId.GenerateNewId().ToString(),"Tên là một dãy liên tiếp không quá 256 kí tự bao gồm chữ số, chữ cái hoặc dấu gạch dưới và bắt đầu bằng?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"chữ số."),
                        new(ObjectId.GenerateNewId().ToString(),"ký tự đặc biệt."),
                        new(ObjectId.GenerateNewId().ToString(),"chữ số hoặc dấu gạch dưới."),
                        new(ObjectId.GenerateNewId().ToString(),"chữ cái hoặc dấu gạch dưới.", true)
                    },
                    "Tin 11 - Gợi ý giải"),
                new(ObjectId.GenerateNewId().ToString(),"Chọn đáp án sai?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Hằng là đại lượng có giá trị không thay đổi trong quá trình thực hiện chương trình."),
                        new(ObjectId.GenerateNewId().ToString(),"Biến là đại lượng được đặt tên, có giá trị không thay đổi trong quá trình thực hiện chương trình.", true),
                        new(ObjectId.GenerateNewId().ToString(),"Tên do người lập trình đặt cần theo nguyên tắc đặt tên của mỗi ngôn ngữ lập trình."),
                        new(ObjectId.GenerateNewId().ToString(),"Tên dành riêng không thể được dùng để đặt tên biến trong chương trình.")
                    },
                    "Tin 11 - Gợi ý giải"),
                new(ObjectId.GenerateNewId().ToString(),"Trường hợp nào dưới đây là tên biến trong Python?", QuestionType.SingleSelection, Level.Medium, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"bai-tap-1"),
                        new(ObjectId.GenerateNewId().ToString(),"baiTap1", true),
                        new(ObjectId.GenerateNewId().ToString(),"_bai tap 1"),
                        new(ObjectId.GenerateNewId().ToString(),"bai tap 1")
                    },
                    "Tin 11 - Gợi ý giải"),
                new(ObjectId.GenerateNewId().ToString(),"Trong Python, kết quả in ra màn hình cuar câu lệnh: print(7*6, '= 7 x 6')", QuestionType.SingleSelection, Level.Medium, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(), "7 * 6 = 42"),
                        new(ObjectId.GenerateNewId().ToString(), "42 = 7 x 6", true),
                        new(ObjectId.GenerateNewId().ToString(), "7 x 6 = 42"),
                        new(ObjectId.GenerateNewId().ToString(), "42 = 7 * 6")
                    },
                    "Tin 11 - Gợi ý giải"),
                new(ObjectId.GenerateNewId().ToString(),"Trong Python, ký hiệu ‘int’ là dữ liệu kiểu:", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"nguyên.", true),
                        new(ObjectId.GenerateNewId().ToString(),"thực."),
                        new(ObjectId.GenerateNewId().ToString(),"xâu."),
                        new(ObjectId.GenerateNewId().ToString(),"lôgic.")
                    },
                    "Tin 11 - Gợi ý giải"),
                new(ObjectId.GenerateNewId().ToString(),"Trong Python, cú pháp: <tên kiểu dữ liệu>(<giá trị>) dùng để?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(), "gán dữ liệu"),
                        new(ObjectId.GenerateNewId().ToString(), "cho biết kiểu dữ liệu"),
                        new(ObjectId.GenerateNewId().ToString(), "gán giá trị"),
                        new(ObjectId.GenerateNewId().ToString(), "chuyển đổi kiểu dữ liệu", true)
                    },
                    "Tin 11 - Gợi ý giải"),
                new(ObjectId.GenerateNewId().ToString(),"Trong Python, cú pháp: type(<tên biến>) sẽ cho biết?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(), "tên biến."),
                        new(ObjectId.GenerateNewId().ToString(), "giá trị của biến."),
                        new(ObjectId.GenerateNewId().ToString(), "kiểu dữ liệu của biến.", true),
                        new(ObjectId.GenerateNewId().ToString(), "danh sách biến.")
                    },
                    "Tin 11 - Gợi ý giải"),
                new("608cd754ef63d3914679ea88","Trong Python, kết quả của biểu thức: (5*2+10) là dữ liệu kiểu?", QuestionType.SingleSelection, Level.Medium, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(), "int.", true),
                        new(ObjectId.GenerateNewId().ToString(), "float."),
                        new(ObjectId.GenerateNewId().ToString(), "str."),
                        new(ObjectId.GenerateNewId().ToString(), "bool.")
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
                new(ObjectId.GenerateNewId().ToString(),"Trong Python, kết quả xuất ra màn hình của chương trình: print('Bac Ai'*4)", QuestionType.SingleSelection, Level.Hardest, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"BacAiBacAiBacAiBacAi"),
                        new(ObjectId.GenerateNewId().ToString(),"BacAi BacAi BacAi BacAi"),
                        new(ObjectId.GenerateNewId().ToString(),"Bac Ai Bac Ai Bac Ai Bac Ai"),
                        new(ObjectId.GenerateNewId().ToString(),"Bac AiBac AiBac AiBac Ai", true)
                    },
                    "Tin 11 - Gợi ý giải"),

                // DANH SÁCH CÂU HỎI QUỐC PHÒNG 10
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

        // THÊM DỮ LIỆU
        private List<Exam> GetPredefinedExams_QP10(string categoryId)
        {
            return new List<Exam>()
            {
                // Tạo đề Quốc phòng 10
                new Exam("Quốc phòng 10 - Ôn tập giữa kỳ 1", "Năm học: 2022-2023",
                    "Ôn tập kiểm tra giữa kỳ 1",
                    15,
                    "10:00",
                    GetPredefinedQuestions_QP10(categoryId).Take(15).ToList(),
                    Level.Easy,
                    true,
                    null,
                    10,
                    true, null, null)
            };
        }
        private List<Question> GetPredefinedQuestions_QP10(string categoryId)
        {
            return new List<Question>()
            {
                new(ObjectId.GenerateNewId().ToString(),"Việc chuẩn bị cho nam thanh niên đủ 17 tuổi nhập ngũ gồm những nội dung nào?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Học tập chính trị, huấn luyện quân sự"),
                        new(ObjectId.GenerateNewId().ToString(),"Huấn luyện quân sự và diễn tập"),
                        new(ObjectId.GenerateNewId().ToString(),"Đăng kí nghĩa vụ quân sự và kiểm tra sức khỏe", true),
                        new(ObjectId.GenerateNewId().ToString(),"Kết nạp Đảng hoặc kết nạp Đoàn cho thanh niên")
                    },
                    "QP10 - Gợi ý giải (nếu có)"),

                new(ObjectId.GenerateNewId().ToString(),"Những trường hợp nào sau đây được hoãn nhập ngũ trong thời bình?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Có anh chị em ruột là hạ sĩ quan, binh sĩ đang phục vụ tại ngũ", true),
                        new(ObjectId.GenerateNewId().ToString(),"Đang nghiên cứu công trình khoa học cấp Bộ"),
                        new(ObjectId.GenerateNewId().ToString(),"Là lao động chính trong gia đình"),
                        new(ObjectId.GenerateNewId().ToString(),"Có anh, chị em ruột là sĩ quan quân đội nhân dân Việt Nam")
                    },
                "QP10 - Gợi ý giải (nếu có)"),

                new(ObjectId.GenerateNewId().ToString(),"Kiểm tra sức khỏe cho những người đăng ký nghĩa vụ quân sự lần đầu là bao nhiêu tuổi?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"16 tuổi"),
                        new(ObjectId.GenerateNewId().ToString(),"17 tuổi", true),
                        new(ObjectId.GenerateNewId().ToString(),"18 tuổi"),
                        new(ObjectId.GenerateNewId().ToString(),"19 tuổi")
                    },
                "QP10 - Gợi ý giải (nếu có)"),

                    new(ObjectId.GenerateNewId().ToString(),"Luật nghĩa vụ quân sự năm 2005 có mấy Chương, Điều?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"10 chương 72 điều"),
                        new(ObjectId.GenerateNewId().ToString(),"10 chương 75 điều"),
                        new(ObjectId.GenerateNewId().ToString(),"11 chương 77 điều"),
                        new(ObjectId.GenerateNewId().ToString(),"11 chương 71 điều", true)
                    },
                "QP10 - Gợi ý giải (nếu có)"),

                new(ObjectId.GenerateNewId().ToString(),"Việc miễn gọi nhập ngũ trong thời chiến do ai hoặc cấp nào qui định?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Bộ Quốc phòng quy định"),
                        new(ObjectId.GenerateNewId().ToString(),"Nhà nước quy định"),
                        new(ObjectId.GenerateNewId().ToString(),"Chính phủ quy định", true),
                        new(ObjectId.GenerateNewId().ToString(),"Ủy ban nhân dân tỉnh quy định")
                    },
                "QP10 - Gợi ý giải (nếu có)"),

                    new(ObjectId.GenerateNewId().ToString(),"Thời hạn phục vụ tại ngũ trong thời bình của hạ sĩ quan chỉ huy, hạ sĩ quan và binh sĩ chuyên môn kỹ thuật do quân đội đào tạo, hạ sĩ quan binh sĩ trên tàu hải quân mấy tháng?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"18 tháng"),
                        new(ObjectId.GenerateNewId().ToString(),"22 tháng"),
                        new(ObjectId.GenerateNewId().ToString(),"24 tháng", true),
                        new(ObjectId.GenerateNewId().ToString(),"36 tháng")
                    },
                "QP10 - Gợi ý giải (nếu có)"),

                new(ObjectId.GenerateNewId().ToString(),"Khi cần thiết, Bộ trưởng Bộ Quốc phòng được quyền giữ hạ sĩ quan, binh sĩ phục vụ tại ngũ thêm một thời gian là mấy tháng so với thời hạn quy định?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Không quá 3 tháng"),
                        new(ObjectId.GenerateNewId().ToString(),"Không quá 6 tháng", true),
                        new(ObjectId.GenerateNewId().ToString(),"Không quá 9 tháng"),
                        new(ObjectId.GenerateNewId().ToString(),"Không quá 12 tháng")
                    },
                "QP10 - Gợi ý giải (nếu có)"),

                new(ObjectId.GenerateNewId().ToString(),"Việc hoãn gọi nhập ngũ và miễn làm nghĩa vụ quân sự theo Luật Nghĩa vụ quân sự 2005 do Ủy ban nhân dân cấp nào quyết định?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Cấp xã"),
                        new(ObjectId.GenerateNewId().ToString(),"Cấp huyện", true),
                        new(ObjectId.GenerateNewId().ToString(),"Cấp tỉnh"),
                        new(ObjectId.GenerateNewId().ToString(),"Cấp thành phố")
                    },
                "QP10 - Gợi ý giải (nếu có)"),

                new(ObjectId.GenerateNewId().ToString(),"Việc khám sức khỏe cho những công dân trong diện được gọi nhập ngũ do Hội đồng khám sức khỏe cấp nào phụ trách?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Cấp huyện, quận, thành phố, thị xã trực thuộc tỉnh", true),
                        new(ObjectId.GenerateNewId().ToString(),"Cấp xã, phường, thị trấn trực thuộc huyện"),
                        new(ObjectId.GenerateNewId().ToString(),"Bệnh xá đơn vị quân đội"),
                        new(ObjectId.GenerateNewId().ToString(),"Bệnh viện trực thuộc tỉnh, bộ, ngành")
                    },
                "QP10 - Gợi ý giải (nếu có)"),

                new(ObjectId.GenerateNewId().ToString(),"Hạn tuổi phục vụ của hạ sĩ quan và binh sĩ ở ngạch dự bị đối với nữ giới là bao nhiêu?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"35 tuổi"),
                        new(ObjectId.GenerateNewId().ToString(),"38 tuổi"),
                        new(ObjectId.GenerateNewId().ToString(),"40 tuổi", true),
                        new(ObjectId.GenerateNewId().ToString(),"42 tuổi")
                    },
                "QP10 - Gợi ý giải (nếu có)"),

                new(ObjectId.GenerateNewId().ToString(),"Những trường hợp nào không được tạm hoãn gọi nhập ngũ trong thời bình?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Học sinh, sinh viên chỉ ghi danh đóng học phí nhưng không học tại trường", true),
                        new(ObjectId.GenerateNewId().ToString(),"Con trai của thương binh hạng 2"),
                        new(ObjectId.GenerateNewId().ToString(),"Học sinh trường trung học phổ thông, trường phổ thông dân tộc nội trú"),
                        new(ObjectId.GenerateNewId().ToString(),"Sinh viên trường trung cấp chuyên nghiệp, trường cao đẳng nghề")
                    },
                "QP10 - Gợi ý giải (nếu có)"),

                new(ObjectId.GenerateNewId().ToString(),"Lãnh thổ quốc gia được cấu thành từ các yếu tố nào?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Lãnh thổ; dân tộc; hiến pháp; pháp luật"),
                        new(ObjectId.GenerateNewId().ToString(),"Lãnh thổ; dân cư; hiến pháp"),
                        new(ObjectId.GenerateNewId().ToString(),"Lãnh thổ; dân tộc; hiến pháp; pháp luật"),
                        new(ObjectId.GenerateNewId().ToString(),"Lãnh thổ; dân cư; nhà nước", true)
                    },
                "QP10 - Gợi ý giải (nếu có)"),

                new(ObjectId.GenerateNewId().ToString(),"Trách nhiệm trong bảo vệ chủ quyền lãnh thổ quốc gia được xác định như thế nào?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Là trách nhiệm của toàn lực lượng vũ trang và toàn dân"),
                        new(ObjectId.GenerateNewId().ToString(),"Là trách nhiệm của toàn Đảng và các tổ chức xã hội"),
                        new(ObjectId.GenerateNewId().ToString(),"Là trách nhiệm của toàn Đảng, toàn quân và toàn dân", true),
                        new(ObjectId.GenerateNewId().ToString(),"Là trách nhiệm của giai cấp, của Đảng và quân đội")
                    },
                "QP10 - Gợi ý giải (nếu có)"),

                new(ObjectId.GenerateNewId().ToString(),"Lãnh thổ thuộc chủ quyền hoàn toàn, tuyệt đối và đầy đủ của quốc gia bao gồm những bộ phận nào?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Vùng đất; vùng nước; vùng trời trên vùng đất, vùng nước; lòng đất dưới chúng", true),
                        new(ObjectId.GenerateNewId().ToString(),"Vùng đất; vùng trời trên vùng đất; lòng đất dưới chúng"),
                        new(ObjectId.GenerateNewId().ToString(),"Vùng đất; vùng nước; vùng trời trên vùng đất; lòng đất dưới chúng"),
                        new(ObjectId.GenerateNewId().ToString(),"Vùng đất; vùng trời; lòng đất dưới chúng")
                    },
                "QP10 - Gợi ý giải (nếu có)"),

                new(ObjectId.GenerateNewId().ToString(),"Vùng lòng đất quốc gia là:", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Toàn bộ phần nằm dưới lòng đất thuộc chủ quyền quốc gia"),
                        new(ObjectId.GenerateNewId().ToString(),"Toàn bộ phần nằm dưới lòng đất, vùng nước thuộc chủ quyền quốc gia", true),
                        new(ObjectId.GenerateNewId().ToString(),"Toàn bộ phần nằm dưới lòng đất, vùng đảo thuộc chủ quyền quốc gia"),
                        new(ObjectId.GenerateNewId().ToString(),"Toàn bộ phần nằm dưới lòng đất, vùng trời thuộc chủ quyền quốc gia")
                    },
                "QP10 - Gợi ý giải (nếu có)")
            };
        }

        private List<Exam> GetPredefinedExams_SinhHoc9(string categoryId)
        {
            return new List<Exam>()
            {
                // Tạo đề Quốc phòng 10
                new Exam("Sinh học 9 - Ôn tập cuối kỳ 1", "Năm học: 2022-2023",
                    "Ôn tập kiểm tra cuối kỳ 1, 2022-2023",
                    10,
                    "10:00",
                    GetPredefinedQuestions_SinhHoc9(categoryId).Take(10).ToList(),
                    Level.Easy,
                    true,
                    null,
                    6,
                    true, null, null)
            };
        }
        private List<Question> GetPredefinedQuestions_SinhHoc9(string categoryId)
        {
            return new List<Question>()
            {
                new(ObjectId.GenerateNewId().ToString(),"Theo Men đen, tính trang được biểu hiện ở F1 gọi là tính trạng:", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Tương ứng"),
                        new(ObjectId.GenerateNewId().ToString(),"Trung gian"),
                        new(ObjectId.GenerateNewId().ToString(),"Lặn"),
                        new(ObjectId.GenerateNewId().ToString(),"Trội", true)
                    },
                    "Sinh học 9 - Gợi ý giải (nếu có)"),

                new(ObjectId.GenerateNewId().ToString(),"Biến dị tổ hợp là:", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"sự tổ hợp lại các tính trạng của P làm xuất hiện kiểu hình khác P", true),
                        new(ObjectId.GenerateNewId().ToString(),"sự tổ hợp lại các tính trạng của P làm xuất hiện kiểu hình giống P"),
                        new(ObjectId.GenerateNewId().ToString(),"sự tổ hợp lại các tính trạng của P làm xuất hiện kiểu gen giống P"),
                        new(ObjectId.GenerateNewId().ToString(),"ự tổ hợp lại các gen của P làm xuất hiện kiểu hình giống P")
                    },
                    "Sinh học 9 - Gợi ý giải (nếu có)"),

                new(ObjectId.GenerateNewId().ToString(),"Hội chứng Tơcnơ ở nữ do mất 1 NST giới tính X, số lượng NST trong tế bào sinh dưỡng là:", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"44"),
                        new(ObjectId.GenerateNewId().ToString(),"45", true),
                        new(ObjectId.GenerateNewId().ToString(),"46"),
                        new(ObjectId.GenerateNewId().ToString(),"47")
                    },
                    "Sinh học 9 - Gợi ý giải (nếu có)"),

                new(ObjectId.GenerateNewId().ToString(),"Trong tế bào sinh dưỡng, thể ba nhiễm của người có số lượng NST là:", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"3"),
                        new(ObjectId.GenerateNewId().ToString(),"45"),
                        new(ObjectId.GenerateNewId().ToString(),"47", true),
                        new(ObjectId.GenerateNewId().ToString(),"49")
                    },
                    "Sinh học 9 - Gợi ý giải (nếu có)"),

                new(ObjectId.GenerateNewId().ToString(),"Phép lai nào sau đây là phép lai phân tích?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Aa x aa", true),
                        new(ObjectId.GenerateNewId().ToString(),"Aa x Aa"),
                        new(ObjectId.GenerateNewId().ToString(),"AA x AA"),
                        new(ObjectId.GenerateNewId().ToString(),"AA x Aa")
                    },
                    "Sinh học 9 - Gợi ý giải (nếu có)"),

                new(ObjectId.GenerateNewId().ToString(),"Ở người, NST số 21 có thêm 1 NST sẽ mắc bệnh gì?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Hồng cầu lưỡi liềm"),
                        new(ObjectId.GenerateNewId().ToString(),"Bệnh Đao", true),
                        new(ObjectId.GenerateNewId().ToString(),"Ung thư máu"),
                        new(ObjectId.GenerateNewId().ToString(),"Hội chứng Tơcnơ")
                    },
                    "Sinh học 9 - Gợi ý giải (nếu có)"),
                new(ObjectId.GenerateNewId().ToString(),"Dạng đột biến cấu trúc sẽ gây ung thư máu ở người là:", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Chuyển đoạn NST 21"),
                        new(ObjectId.GenerateNewId().ToString(),"Lặp đoạn NST 21"),
                        new(ObjectId.GenerateNewId().ToString(),"Đảo đoạn NST 21"),
                        new(ObjectId.GenerateNewId().ToString(),"Mất đoạn NST 21", true)
                    },
                    "Sinh học 9 - Gợi ý giải (nếu có)"),

                new(ObjectId.GenerateNewId().ToString(),"Hậu quả của loại đột biến nào sau đây gây ra bệnh Đao là:", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Đột biến dị bội thể"),
                        new(ObjectId.GenerateNewId().ToString(),"Đột biến gen lặn", true),
                        new(ObjectId.GenerateNewId().ToString(),"Đột biến cấu trúc NST"),
                        new(ObjectId.GenerateNewId().ToString(),"Đột biến đa bội thể")
                    },
                    "Sinh học 9 - Gợi ý giải (nếu có)"),

                new(ObjectId.GenerateNewId().ToString(),"Cây cà độc dược lưỡng bội có bộ NST 2n = 24. Dạng dị bội thể (2n -1) của chúng có số lượng là:", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"23 NST", true),
                        new(ObjectId.GenerateNewId().ToString(),"24 NST"),
                        new(ObjectId.GenerateNewId().ToString(),"25 NST"),
                        new(ObjectId.GenerateNewId().ToString(),"26 NST")
                    },
                    "Sinh học 9 - Gợi ý giải (nếu có)"),


                new(ObjectId.GenerateNewId().ToString(),"Thường biến thuộc loại biến dị nào sau đây?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Biến dị di truyền"),
                        new(ObjectId.GenerateNewId().ToString(),"Biến dị tổ hợp"),
                        new(ObjectId.GenerateNewId().ToString(),"Biến dị không di truyền", true),
                        new(ObjectId.GenerateNewId().ToString(),"Biến dị số lượng NST")
                    },
                    "Sinh học 9 - Gợi ý giải (nếu có)")
            };
        }

        private List<Exam> GetPredefinedExams_VatLy7(string categoryId)
        {
            return new List<Exam>()
            {
                // Vật lý 7
                new Exam("Vật lý 7 - Ôn tập cuối kỳ 1", "Năm học: 2022-2023",
                    "Ôn tập kiểm tra cuối kỳ 1, 2022-2023",
                    8,
                    "08:00",
                    GetPredefinedQuestions_VatLy7(categoryId).Take(8).ToList(),
                    Level.Easy,
                    true,
                    null,
                    5,
                    true, null, null)
            };
        }
        private List<Question> GetPredefinedQuestions_VatLy7(string categoryId)
        {
            return new List<Question>()
            {
                new(ObjectId.GenerateNewId().ToString(),"Trong hệ đơn vị đo lường chính thức ở nước ta, đơn vị tốc độ là", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"m/s và km/h", true),
                        new(ObjectId.GenerateNewId().ToString(),"m/min và km/h"),
                        new(ObjectId.GenerateNewId().ToString(),"cm/s và m/s"),
                        new(ObjectId.GenerateNewId().ToString(),"mm/s và m/s")
                    },
                    "Vật lý 7 - Gợi ý giải (nếu có)"),

                new(ObjectId.GenerateNewId().ToString(),"Công thức tính tốc độ là", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"v = t/s"),
                        new(ObjectId.GenerateNewId().ToString(),"V = F.d"),
                        new(ObjectId.GenerateNewId().ToString(),"V= F/d"),
                        new(ObjectId.GenerateNewId().ToString(),"v = s/t", true)
                    },
                    "Vật lý 7 - Gợi ý giải (nếu có)"),
                 new(ObjectId.GenerateNewId().ToString(),"Khi 1 người thổi sáo, tiếng sáo được tạo ra bởi sự dao động của", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"đôi môi của người thổi"),
                        new(ObjectId.GenerateNewId().ToString(),"thành ống sáo"),
                        new(ObjectId.GenerateNewId().ToString(),"các ngón tay của người thổi"),
                        new(ObjectId.GenerateNewId().ToString(),"cột không khí trong ống sáo", true)
                    },
                    "Vật lý 7 - Gợi ý giải (nếu có)"),

                new(ObjectId.GenerateNewId().ToString(),"Đơn vị của tần số là", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"dB"),
                        new(ObjectId.GenerateNewId().ToString(),"Hz", true),
                        new(ObjectId.GenerateNewId().ToString(),"N"),
                        new(ObjectId.GenerateNewId().ToString(),"kg")
                    },
                    "Vật lý 7 - Gợi ý giải (nếu có)"),
                 new(ObjectId.GenerateNewId().ToString(),"Khi độ to của vật tăng thì biên độ dao động âm của vật sẽ biến đổi như thế nào?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Giảm"),
                        new(ObjectId.GenerateNewId().ToString(),"Không thay đổi"),
                        new(ObjectId.GenerateNewId().ToString(),"Tăng", true),
                        new(ObjectId.GenerateNewId().ToString(),"Vừa tăng vừa giảm")
                    },
                    "Vật lý 7 - Gợi ý giải (nếu có)"),

                new(ObjectId.GenerateNewId().ToString(),"Vật nào sau đây phản xạ âm tốt?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Miếng xốp"),
                        new(ObjectId.GenerateNewId().ToString(),"Rèm nhung"),
                        new(ObjectId.GenerateNewId().ToString(),"Mặt Gương", true),
                        new(ObjectId.GenerateNewId().ToString(),"Đệm cao su")
                    },
                    "Vật lý 7 - Gợi ý giải (nếu có)"),

                new(ObjectId.GenerateNewId().ToString(),"Khi em nghe được tiếng nói của mình vang lại trong hang động nhiều lần, điều đó có ý nghĩa gì?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Tiếng nói của em gặp vật cản bị phản xạ và lập lại", true),
                        new(ObjectId.GenerateNewId().ToString(),"Trong hang động có mối nguy hiểm"),
                        new(ObjectId.GenerateNewId().ToString(),"Có người ở trong hang cũng đang nói to"),
                        new(ObjectId.GenerateNewId().ToString(),"Sóng âm truyền đi trong hang quá nhanh")
                    },
                    "Vật lý 7 - Gợi ý giải (nếu có)"),

                 new(ObjectId.GenerateNewId().ToString(),"Hiện tượng nào sau đây không liên quan đến năng lượng ánh sáng?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Ánh sáng mặt trời làm cháy bỏng da"),
                        new(ObjectId.GenerateNewId().ToString(),"Ánh sáng mặt trời phản chiếu trên mặt nước", true),
                        new(ObjectId.GenerateNewId().ToString(),"Bếp mặt trời nóng lên nhờ ánh sáng mặt trời"),
                        new(ObjectId.GenerateNewId().ToString(),"Ánh sáng mặt trời dùng để tạo điện năng")
                    },
                    "Vật lý 7 - Gợi ý giải (nếu có)"),

                new(ObjectId.GenerateNewId().ToString(),"Ảnh của 1 vật tạo bởi gương phẳng, có tính chất là", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"ảnh ảo, lớn hơn vật"),
                        new(ObjectId.GenerateNewId().ToString(),"ảnh ảo, bé hơn vật"),
                        new(ObjectId.GenerateNewId().ToString(),"ảnh ảo, bằng vật"),
                        new(ObjectId.GenerateNewId().ToString(),"ảnh thật, bằng vật", true)
                    },
                    "Vật lý 7 - Gợi ý giải (nếu có)"),

               new(ObjectId.GenerateNewId().ToString(),"Một xe máy đang chạy với vận tốc 40km/h. khoảng cách an toàn tối thiểu của người đi xe so với xe trước là:", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"30m"),
                        new(ObjectId.GenerateNewId().ToString(),"33,3m", true),
                        new(ObjectId.GenerateNewId().ToString(),"40m"),
                        new(ObjectId.GenerateNewId().ToString(),"20m")
                    },
                    "Vật lý 7 - Gợi ý giải (nếu có)")
             };
        }

        private List<Exam> GetPredefinedExams_TinHoc12(string categoryId)
        {
            return new List<Exam>()
            {
                // Vật lý 7
                new Exam("Tin học 12 - Ôn tập giữa kỳ", "Năm học: 2022-2023",
                    "Ôn tập kiểm tra cuối kỳ 1, 2022-2023",
                    10,
                    "05:00",
                    GetPredefinedQuestions_TinHoc12(categoryId).Take(10).ToList(),
                    Level.Medium,
                    true,
                    null,
                    5,
                    true, null, null)
            };
        }
        private List<Question> GetPredefinedQuestions_TinHoc12(string categoryId)
        {
            return new List<Question>()
            {
                new(ObjectId.GenerateNewId().ToString(),"Hệ quản trị CSDL là:", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Phần mềm dùng tạo lập, cập nhật, lưu trữ và khai thác thông tin của CSDL", true),
                        new(ObjectId.GenerateNewId().ToString(),"Phần mềm dùng tạo lập, lưu trữ một CSDL"),
                        new(ObjectId.GenerateNewId().ToString(),"Phần mềm để thao tác và xử lý các đối tượng trong CSDL"),
                        new(ObjectId.GenerateNewId().ToString(),"Phần mềm dùng tạo lập CSDL")
                    },
                    "Tin học 12 - Đọc SGK từ bài 1-7"),
                new(ObjectId.GenerateNewId().ToString(),"Xét công tác quản lí hồ sơ. Trong số các công việc sau, những việc nào không thuộc nhóm thao tác cập nhật hồ sơ?", QuestionType.SingleSelection, Level.Medium, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Xóa một hồ sơ"),
                        new(ObjectId.GenerateNewId().ToString(),"Thống kê và lập báo cáo", true),
                        new(ObjectId.GenerateNewId().ToString(),"Thêm hai hồ sơ"),
                        new(ObjectId.GenerateNewId().ToString(),"Sửa tên trong một hồ sơ")
                    },
                    "Tin học 12 - Đọc SGK từ bài 1-7"),
                 new(ObjectId.GenerateNewId().ToString(),"Quy trình xây dựng CSDL là:", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Khảo sát --> Thiết kế --> Kiểm thử", true),
                        new(ObjectId.GenerateNewId().ToString(),"Khảo sát --> Kiểm thử --> Thiết kế"),
                        new(ObjectId.GenerateNewId().ToString(),"Thiết kế --> Kiểm thử --> Khảo sát"),
                        new(ObjectId.GenerateNewId().ToString(),"Thiết kế --> Khảo sát --> Kiểm thử")
                    },
                    "Tin học 12 - Đọc SGK từ bài 1-7"),
                new(ObjectId.GenerateNewId().ToString(),"Người nào đã tạo ra các phần mềm ứng dụng đáp ứng nhu cầu khai thác thông tin từ CSDL?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Người dùng"),
                        new(ObjectId.GenerateNewId().ToString(),"Người quản trị CSDL"),
                        new(ObjectId.GenerateNewId().ToString(),"Người bảo hành các thiết bị phần cứng"),
                        new(ObjectId.GenerateNewId().ToString(),"Người lập trình ứng dụng", true)
                    },
                    "Tin học 12 - Đọc SGK từ bài 1-7"),
                new(ObjectId.GenerateNewId().ToString(),"Đối tượng nào tạo giao diện thuận tiện cho việc nhập hoặc hiển thị thông tin?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Table"),
                        new(ObjectId.GenerateNewId().ToString(),"Query"),
                        new(ObjectId.GenerateNewId().ToString(),"Form", true),
                        new(ObjectId.GenerateNewId().ToString(),"Report")
                    },
                    "Tin học 12 - Đọc SGK từ bài 1-7"),
                new(ObjectId.GenerateNewId().ToString(),"Phần mở rộng của tệp trong Access 2019 là", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"ACCDB", true),
                        new(ObjectId.GenerateNewId().ToString(),"DOCX"),
                        new(ObjectId.GenerateNewId().ToString(),"XLSX"),
                        new(ObjectId.GenerateNewId().ToString(),"TXT")
                    },
                    "Tin học 12 - Đọc SGK từ bài 1-7"),
                new(ObjectId.GenerateNewId().ToString(),"Chọn câu sai trong các câu sau:", QuestionType.SingleSelection, Level.Medium, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Access có khả năng cung cấp công cụ tạo lập CSDL"),
                        new(ObjectId.GenerateNewId().ToString(),"Access không hỗ trợ lưu trữ CSDL trên các thiết bị nhớ", true),
                        new(ObjectId.GenerateNewId().ToString(),"Access cho phép cập nhật dữ liệu, tạo báo cáo, thống kê, tổng hợp"),
                        new(ObjectId.GenerateNewId().ToString(),"CSDL xây dựng trong Access gồm các bảng và liên kết giữa các bảng")
                    },
                    "Tin học 12 - Đọc SGK từ bài 1-7"),
                new(ObjectId.GenerateNewId().ToString(),"Muốn sắp xếp các bản ghi thứ tự giảm dần, thực hiện lệnh nào?", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Home --> Sort Ascending"),
                        new(ObjectId.GenerateNewId().ToString(),"File --> Sort Ascending"),
                        new(ObjectId.GenerateNewId().ToString(),"Home --> Sort Descending", true),
                        new(ObjectId.GenerateNewId().ToString(),"File --> Sort Descending")
                    },
                    "Tin học 12 - Đọc SGK từ bài 1-7"),
                new(ObjectId.GenerateNewId().ToString(),"Khi liên kết bị sai, ta có thể sửa lại bằng cách chọn đường liên kết cần sửa, sau đó:", QuestionType.SingleSelection, Level.Easy, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"Nháy đúp vào đường liên kết --> Chọn lại trường cần liên kết", true),
                        new(ObjectId.GenerateNewId().ToString(),"Edit --> Relationships"),
                        new(ObjectId.GenerateNewId().ToString(),"Tools --> Relationships --> Change Field"),
                        new(ObjectId.GenerateNewId().ToString(),"Nhấn phímDelete --> Yes")
                    },
                    "Tin học 12 - Đọc SGK từ bài 1-7"),
                new(ObjectId.GenerateNewId().ToString(),"Trong Access, biểu thức điều kiện nào sau đây là sai?", QuestionType.SingleSelection, Level.Medium, categoryId,
                    new List<Answer>()
                    {
                        new(ObjectId.GenerateNewId().ToString(),"[GT]=”Nam” and [Tin]>=8.5"),
                        new(ObjectId.GenerateNewId().ToString(),"[SoDan]/[DienTich]"),
                        new(ObjectId.GenerateNewId().ToString(),"[LUONG]*0.1"),
                        new(ObjectId.GenerateNewId().ToString(),"([TOAN] + [VAN]):2", true)
                    },
                    "Tin học 12 - Đọc SGK từ bài 1-7"),
             };
        }
    }
}