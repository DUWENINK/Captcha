﻿using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DUWENINK.Captcha.Extensions;
using SixLabors.ImageSharp.Drawing.Processing;

namespace DUWENINK.Captcha
{
    /// <summary>
    /// 验证码配置和逻辑
    /// </summary>
    public class SecurityCodeHelper: ISecurityCodeHelper
    {
        /// <summary>
        /// 验证码文本池
        /// </summary>
        private static readonly string[] _cnTextArr = new string[] { "的", "一", "国", "在", "人", "了", "有", "中", "是", "年", "和", "大", "业", "不", "为", "发", "会", "工", "经", "上", "地", "市", "要", "个", "产", "这", "出", "行", "作", "生", "家", "以", "成", "到", "日", "民", "来", "我", "部", "对", "进", "多", "全", "建", "他", "公", "开", "们", "场", "展", "时", "理", "新", "方", "主", "企", "资", "实", "学", "报", "制", "政", "济", "用", "同", "于", "法", "高", "长", "现", "本", "月", "定", "化", "加", "动", "合", "品", "重", "关", "机", "分", "力", "自", "外", "者", "区", "能", "设", "后", "就", "等", "体", "下", "万", "元", "社", "过", "前", "面", "农", "也", "得", "与", "说", "之", "员", "而", "务", "利", "电", "文", "事", "可", "种", "总", "改", "三", "各", "好", "金", "第", "司", "其", "从", "平", "代", "当", "天", "水", "省", "提", "商", "十", "管", "内", "小", "技", "位", "目", "起", "海", "所", "立", "已", "通", "入", "量", "子", "问", "度", "北", "保", "心", "还", "科", "委", "都", "术", "使", "明", "着", "次", "将", "增", "基", "名", "向", "门", "应", "里", "美", "由", "规", "今", "题", "记", "点", "计", "去", "强", "两", "些", "表", "系", "办", "教 正", "条", "最", "达", "特", "革", "收", "二", "期", "并", "程", "厂", "如", "道", "际 及", "西", "口", "京", "华", "任", "调", "性", "导", "组", "东", "路", "活", "广", "意", "比", "投", "决", "交", "统", "党", "南", "安", "此", "领", "结", "营", "项", "情", "解", "议", "义", "山", "先", "车", "然", "价", "放", "世", "间", "因", "共", "院", "步", "物", "界", "集", "把", "持", "无", "但", "城", "相", "书", "村", "求", "治", "取", "原", "处", "府", "研", "质", "信", "四", "运", "县", "军", "件", "育", "局", "干", "队", "团", "又", "造", "形", "级", "标", "联", "专", "少", "费", "效", "据", "手", "施", "权", "江", "近", "深", "更", "认", "果", "格", "几", "看", "没", "职", "服", "台", "式", "益", "想", "数", "单", "样", "只", "被", "亿", "老", "受", "优", "常", "销", "志", "战", "流", "很", "接", "乡", "头", "给", "至", "难", "观", "指", "创", "证", "织", "论", "别", "五", "协", "变", "风", "批", "见", "究", "支", "那", "查", "张", "精", "每", "林", "转", "划", "准", "做", "需", "传", "争", "税", "构", "具", "百", "或", "才", "积", "势", "举", "必", "型", "易", "视", "快", "李", "参", "回", "引", "镇", "首", "推", "思", "完", "消", "值", "该", "走", "装", "众", "责", "备", "州", "供", "包", "副", "极", "整", "确", "知", "贸", "己", "环", "话", "反", "身", "选", "亚", "么", "带", "采", "王", "策", "真", "女", "谈", "严", "斯", "况", "色", "打", "德", "告", "仅", "它", "气", "料", "神", "率", "识", "劳", "境", "源", "青", "护", "列", "兴", "许", "户", "马", "港", "则", "节", "款", "拉", "直", "案", "股", "光", "较", "河", "花", "根", "布", "线", "土", "克", "再", "群", "医", "清", "速", "律", "她", "族", "历", "非", "感", "占", "续", "师", "何", "影", "功", "负", "验", "望", "财", "类", "货", "约", "艺", "售", "连", "纪", "按", "讯", "史", "示", "象", "养", "获", "石", "食", "抓", "富", "模", "始", "住", "赛", "客", "越", "闻", "央", "席", "坚" };
        private static readonly string[] _enTextArr = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "k", "m", "n", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        /// <summary>
        /// 验证码图片宽高
        /// </summary>
        private readonly int _imageWidth = 120;
        private readonly int _imageHeight = 50;
        private readonly int _fontHeight = 24;

        /// <summary>
        /// 泡泡数量
        /// </summary>
        private int _circleCount = 10;
        /// <summary>
        /// 泡泡半径范围
        /// </summary>
        private readonly int _miniCircleR = 2;
        private readonly int _maxCircleR = 8;

        /// <summary>
        /// 颜色池,较深的颜色
        /// https://tool.oschina.net/commons?type=3
        /// </summary>
        private static readonly string[] _colorHexArr = new string[] { "#00E5EE", "#000000", "#2F4F4F", "#000000", "#43CD80", "#191970", "#006400", "#458B00", "#8B7765", "#CD5B45" };
        ///较浅的颜色
        private static readonly string[] _lightColorHexArr = new string[] { "#FFFACD", "#FDF5E6", "#F0FFFF", "#BBFFFF", "#FAFAD2", "#FFE4E1", "#DCDCDC", "#F0E68C" };

        private static readonly Random _random = new Random();

        /// <summary>
        /// 字体池
        /// </summary>
       // private static Font[] _fontArr;

        private static Font _textFont;

        public SecurityCodeHelper()
        {
            InitFonts(_fontHeight);
        }

        /// <summary>
        /// 生成随机中文字符串
        /// </summary>
        /// <param name="lenght"></param>
        /// <returns></returns>
        public string GetRandomCnText(int length)
        {
            var sb = new StringBuilder();
            if (length <= 0) return sb.ToString();
            do
            {
                sb.Append(_cnTextArr[_random.Next(0, _cnTextArr.Length)]);
            }
            while (--length > 0);
            return sb.ToString();
        }

        /// <summary>
        /// 生成随机英文字母/数字组合字符串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string GetRandomEnDigitalText(int length)
        {
            var sb = new StringBuilder();
            if (length <= 0) return sb.ToString();
            do
            {
                if (_random.Next(0, 2) > 0)
                {
                    sb.Append(_random.Next(2, 10));
                }
                else
                {
                    sb.Append(_enTextArr[_random.Next(0, _enTextArr.Length)]);
                }
            }
            while (--length > 0);
            return sb.ToString();
        }

        /// <summary>
        /// 获取泡泡样式验证码
        /// </summary>
        /// <param name="text">2-3个文字，中文效果较好</param>
        /// <returns>验证码图片字节数组</returns>
        public byte[] GetBubbleCodeByte(string text)
        {
            using Image<Rgba32> img = new(_imageWidth, _imageHeight);
            

            var colorCircleHex = _colorHexArr[_random.Next(0, _colorHexArr.Length)];
            var colorTextHex = colorCircleHex;

            if (_random.Next(0, 6) == 3)
            {
                colorCircleHex = "#FFFFFF";//白色
                _circleCount = (int)(_circleCount * 2.65);
            }
            var white = new Rgba32(255, 255, 255); // 创建白色

            img.Mutate(ctx => ctx
                    .Fill(white)
                    .DrawingCnText(_imageWidth, _imageHeight, text, ParseHexColor(colorTextHex), _textFont)
                    .DrawingCircles(_imageWidth, _imageHeight, _circleCount, _miniCircleR, _maxCircleR, ParseHexColor(colorCircleHex))
                );

            using var ms = new MemoryStream();
            img.Save(ms, PngFormat.Instance);
            return ms.ToArray();
        }

        public static Rgba32 ParseHexColor(string hexColor)
        {
            // 移除颜色字符串中可能存在的'#'字符
            hexColor = hexColor.TrimStart('#');

            // 解析R, G, B 值
            var r = Convert.ToByte(hexColor[..2], 16);
            var g = Convert.ToByte(hexColor.Substring(2, 2), 16);
            var b = Convert.ToByte(hexColor.Substring(4, 2), 16);
            byte a = 255; // 默认不透明

            // 如果提供了Alpha值，则解析Alpha值
            if (hexColor.Length == 8)
            {
                a = Convert.ToByte(hexColor.Substring(6, 2), 16);
            }

            return new Rgba32(r, g, b, a);
        }

        /// <summary>
        /// 英文字母+数字组合验证码
        /// </summary>
        /// <param name="text"></param>
        /// <returns>验证码图片字节数组</returns>
        public byte[] GetEnDigitalCodeByte(string text)
        {
            using Image<Rgba32> img = new(_imageWidth, _imageHeight);
            var colorTextHex = _colorHexArr[_random.Next(0, _colorHexArr.Length)];
            var lignthColorHex = _lightColorHexArr[_random.Next(0, _lightColorHexArr.Length)];
            var white = new Rgba32(255, 255, 255); // 创建白色

            img.Mutate(ctx => ctx
                    .Fill(ParseHexColor(_lightColorHexArr[_random.Next(0, _lightColorHexArr.Length)]))
                    .Glow(ParseHexColor(lignthColorHex))
                    .DrawingGrid(_imageWidth, _imageHeight, ParseHexColor(lignthColorHex), 5, 1)
                    .DrawingCnText(_imageWidth, _imageHeight, text, ParseHexColor(colorTextHex), _textFont)
                    .GaussianBlur(0.1f)
                    .DrawingCircles(_imageWidth, _imageHeight, 15, _miniCircleR, _maxCircleR, white)
                );

            using var ms = new MemoryStream();
            img.Save(ms, PngFormat.Instance);
            return ms.ToArray();


      
        }

        /// <summary>
        /// 初始化字体池
        /// </summary>
        /// <param name="fontSize">一个初始大小</param>
        private static void InitFonts(int fontSize)
        {
            if (_textFont != null) return;
            var list = new List<Font>();

            var sourceDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,  "fonts");
            if (Directory.Exists(sourceDir))
            {
                var fontFiles = Directory.GetFiles(sourceDir, "*.ttf");
                list.AddRange(from fontFile in fontFiles let fontCollection = new FontCollection() select new Font(fontCollection.Add(fontFile), fontSize));
            }
            else
            {
                //尝试读取嵌入资源
                var assembly = typeof(SecurityCodeHelper).Assembly;
                var resourceNames = assembly.GetManifestResourceNames();
                foreach (var resourceName in resourceNames)
                {
                    if (!resourceName.EndsWith(".ttf")) continue;
                    using var stream = assembly.GetManifestResourceStream(resourceName);
                    if (stream == null) continue;
                    var fontCollection = new FontCollection();
                    list.Add(new Font(fontCollection.Add(stream), fontSize));
                }
                //throw new Exception($"绘制验证码字体文件不存在，请将字体文件(.ttf)复制到目录：{sourceDir}");
            }
         

            
            _textFont = list.FirstOrDefault(c => "STCAIYUN".Equals(c.Name, StringComparison.CurrentCultureIgnoreCase));
            _textFont ??= list[_random.Next(0, list.Count)];
        }

    }
}
 