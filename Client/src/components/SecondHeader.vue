<template>
    <q-header class="layout-header" id="secondHeader" reveal elevated>
      <q-toolbar class="layout-toolbar">
        <q-btn value="1">1</q-btn>
        <q-toolbar-title></q-toolbar-title>
        <q-btn value="2">2</q-btn>
      </q-toolbar>      
    </q-header>
</template>

<script>

export default {
    name: "SecondHeader",
    props:{          
      rightDrawerOpen : Boolean,
      rightDrawerIs : Boolean
    },
    methods:{
        addSecondHeader : function() {
          var firstHeader = document.getElementById("firstHeader");
          firstHeader.style.zIndex = 2;
          var height = window.getComputedStyle(firstHeader).height;      
          
          var secondHeader = document.getElementById("secondHeader");
          secondHeader.style.top = height;
          secondHeader.style.zIndex = 1;           
          
          var qPage = document.getElementById("page-container");             
          qPage.style.paddingTop = parseInt(height) + "px";
        },
        setRightPadding :function(){
          var qdrawer = document.getElementsByClassName("q-drawer")[0];          
          var width = qdrawer.style.width;                   
          
          var secHeader = document.getElementById("secondHeader");                            
          secHeader.style.paddingRight = width;
        },
        checkRightDrawer : function(){        
            
            if(!this.rightDrawerIs){
              document.getElementById("secondHeader").style.paddingRight = "";              
              return;
            }            

            if(this.rightDrawerOpen){   

              if(document.getElementsByTagName("footer")[0].getAttribute("style").right){
                document.getElementById("secondHeader").style.paddingRight = "";
              }
              else
                this.setRightPadding();
                               
            }
            else{
              document.getElementById("secondHeader").style.paddingRight = "";
            }                       
        }      
    },

    mounted(){
      this.addSecondHeader();         
      this.checkRightDrawer();  
    },    

    watch : {     
      rightDrawerOpen : function(){
        this.checkRightDrawer();           
      },
      rightDrawerIs : function(){
        this.checkRightDrawer();
      }
    }
}

</script>
