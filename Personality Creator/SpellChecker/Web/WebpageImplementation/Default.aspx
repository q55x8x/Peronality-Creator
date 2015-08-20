<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="WebpageImplementation._Default" %>

<%

    WebpageImplementation.Misc.ReferenceJSFile(Page, Me.ResolveUrl("js/jquery.js"))
    WebpageImplementation.Misc.ReferenceJSFile(Page, Me.ResolveUrl("js/ajax.js"))
    WebpageImplementation.Misc.ReferenceJSFile(Page, Me.ResolveUrl("js/jquery.timer.js"))

%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>

Test 1:<div class="i00SpellCheck"></div><br>
Test 2:<div class="i00SpellCheck i00Multiline" style="width:100%">hello there errror</div>



 <script type="text/javascript">
 
 //GET SET CARRET RANGE:
 
    getCharacterOffsetWithin = function (range, node) {
        var treeWalker = document.createTreeWalker(
        node,
        NodeFilter.SHOW_TEXT,
        function(node) {
            var nodeRange = document.createRange();
            nodeRange.selectNodeContents(node);
            return nodeRange.compareBoundaryPoints(Range.END_TO_END, range) < 1 ? NodeFilter.FILTER_ACCEPT : NodeFilter.FILTER_REJECT;
        },
        false
        );

        var charCount = 0;
        while (treeWalker.nextNode()) {
            charCount += treeWalker.currentNode.length;
        }
        if (range.startContainer.nodeType == 3) {
            charCount += range.startOffset;
        }
        return charCount;
    }
 
 
 
 
 
 
    var SpellErrors = new Array();
    var SpellOK = new Array();
    var SpellPending = new Array();
    var SpellPendingSent = new Array();

    //word right click
    $(".SpellError").mousedown(function(e) {
        alert("error click");
    });

    //sends all words in SpellPending to the server for checking
    var SendWordsToServer = function (){
        if(SpellPending.length==0){
            //no new words to send back ... but still need to update document errors
            UpdateCorrections();
        }else{
            //send to server to spell check
            var URL="<% = Me.ResolveUrl("CheckSpelling.aspx") %>?Words=" + SpellPending.join(";");
            SpellPendingSent = SpellPendingSent.concat(SpellPending);
            SpellPending = new Array();
            Ajax(URL);
        }
    }
    
    //called by the server when results are returned
    //also updates the squiggles under the erroneous words
    var UpdateCorrections = function(OKWords,ErrorWords){
        SpellOK = SpellOK.concat(OKWords);
        SpellErrors = SpellErrors.concat(ErrorWords);
        
        $(".i00SpellCheck").each(function(index){
//            //get carret position qwertyuiop
//            var range = window.getSelection().getRangeAt(0);
//            var oldCarretPos = getCharacterOffsetWithin(range, this);
            //alert(oldCarretPos);
            $(this).attr("contenteditable",false);
            //unmark existing errors
            $(".SpellError", this).each(function(index){
                $(this).contents().unwrap();
            });
            //and underline errors...
            UpdateCorrectionsRecur(this);
            $(this).attr("contenteditable",true);
        });
    }
    //adds the error lines in
    var UpdateCorrectionsRecur = function(el){
        //loop through our elements
        for(var startIndex=0,endIndex=el.childNodes.length;startIndex<endIndex;startIndex++) {
            var item = el.childNodes[startIndex];
            //find words that...
            if ( item.nodeType != 8 ) {//... are not a comment
                if(item.nodeType==3) {//... are text
                    //split the word group into words...
                    var text = item.data;
                    var newtext="",match,index=0;
                    var regex = new RegExp("(\\b\\w+?\\b)", "gi")
                    regex.lastIndex = 0;
                    while(match = regex.exec(text)) {
                        var word=text.substr(match.index,match[0].length);
                        if($.inArray(word, SpellErrors)!=-1){
                            newtext += text.substr(index,match.index-index)+"<span class='SpellError' onmousedown='CheckSpelling(this)'>"+word+"</span>";
                            index = match.index+match[0].length;                        
                        }
                    }
                    if(newtext) {
                        //add the last part of the text
                        newtext += text.substring(index);
                        var repl = $.merge([],$("<span>"+newtext+"</span>")[0].childNodes);
                        endIndex += repl.length-1;
                        startIndex += repl.length-1;
                        $(item).before(repl).remove();
                    }                

                } else {
                    if(item.nodeType==1) {
                        UpdateCorrectionsRecur(item);
                    }
                }
            }
        }
    }
 
    //Go through our element and get phase the words to be checked
    var PhaseWords = function(el) {
        //first remove errors so we can spellcheck reliably
        $(el).attr("contenteditable",false);
        $(".SpellError", el).each(function(index){
            $(this).contents().unwrap();
        });
        $(el).attr("contenteditable",true);
        //alert("asd");
        PhaseWordsRecur(el);
        SendWordsToServer();
    }
    var PhaseWordsRecur = function(el) {
        //loop through our elements
        for(var startIndex=0,endIndex=el.childNodes.length;startIndex<endIndex;startIndex++) {
            var item = el.childNodes[startIndex];
            //find words that...
            if ( item.nodeType != 8 ) {//... are not a comment
                if(item.nodeType==3) {//... are text
                    //split the word group into words...
                    var words=item.data.match(/(\b\w+?\b)/g);
                    if(words!==null){
                        for (var i = 0; i < words.length; i++) {
                            var word=words[i];
                            //for words that have not been spell checked...
                            if($.inArray(word, SpellErrors)!=-1||$.inArray(word, SpellOK)!=-1||$.inArray(word, SpellPending)!=-1||$.inArray(word, SpellPendingSent)!=-1){
                                //word has been checked before
                            } else {
                                //add word to list to be spell checked...
                                SpellPending[SpellPending.length]=word;
                            }
                        }
                    }
                } else {
                    if(item.nodeType==1) {
                        PhaseWordsRecur(item);
                    }
                }
            }
        }
    }

    //Setup i00 Spell Check
    $(document).ready(function(){

        //Styles...
        //Add the spell check error style
        $("<style type='text/css'>.SpellError{background-image:url('line.aspx'); background-repeat:repeat-x;background-position:left bottom;}</style>").appendTo("head");
        //setup a standard textbox like style
        $("<style type='text/css'>.i00SpellCheck{border:1px solid ButtonShadow;overflow-x:hidden;color:windowtext;width:100px;display:inline-block} </style>").appendTo("head");
        //for multiline
        $("<style type='text/css'>.i00Multiline{min-height:64px} </style>").appendTo("head");

        //go through all div's with i00SpellCheck attribute
        $(".i00SpellCheck").each(function(index){
        
            //other attributes...
            var multiline = $(this).hasClass("i00Multiline");

            //turn off built in spellchecking and make it editable
            $(this).attr("spellcheck",false);

            //and make it editable
            $(this).attr("contenteditable",true);

            //specific style for when NOT multiline
            if (!multiline) {
                $(this).css("white-space", "nowrap");
            }
            
            //to fix an issue that occurs when some browsers when the div has no content the height will otherwise be 0px
            if($(this).html()==""){
                $(this).html("&nbsp;");
                $(this).css("min-height",$(this).css("height"));
                $(this).html("");
            }
            $(this).css("height","");
            
            //multiline support
            if (!multiline) {
                //disable new line on enter
                $(this).keypress(function(e){
                    return e.which != 13;
                });
                //remove all breaks (p / br) on lost focus (for copy and pastes)
                $(this).blur(function(e){
                    $(this).find($("p")).each(function(index){
                        $(this).contents().unwrap();
                    });
                    $(this).find($("br")).each(function(index){
                        $(this).remove();
                    });                        
                });
            }
            
            //spellcheck support - check for change
            //1 second timer - check only after 1/2 second of inactivity
            this.timer = $.timer(function() {
                this.stop();
                PhaseWords(this.control);
            });
            this.timer.control=this;
            this.timer.set({time:500,autostart:false});
            $(this).keypress(function(e){
                this.timer.stop();
                this.timer.play(true);
            });
             
            //spellcheck initial content
            PhaseWords(this);
        
        });
    })
 </script>
  
 <a href=# onclick="removeSpellingTags()">click</a>
<script language="javascript" type="text/javascript">
    function removeSpellingTags(){
        $(".i00SpellCheck").each(function(index){
            alert($(this).html());
        });
    }
</script>

  
    
</body>
</html>
