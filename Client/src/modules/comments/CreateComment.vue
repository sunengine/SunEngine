<template>
	<div class="create-comment">
		<SunEditor
			content-class="material-text"
			:toolbar="editorToolbar"
			:rules="commentRules"
			class="create-comment__editor"
			ref="htmlEditor"
			v-model="comment.text"
		/>
		<div>
			<q-btn
				class="send-btn"
				:icon="$iconsSet.CreateComment.send"
				no-caps
				@click="addComment"
				:loading="loading"
				:label="$tl('sendBtn')"
			>
				<LoaderSent slot="loading" />
			</q-btn>
		</div>
	</div>
</template>

<script>
import { htmlTextSizeOrHasImage } from "sun";

export default {
	name: "CreateComment",
	data() {
		return {
			comment: {
				materialId: this.materialId,
				text: ""
			},
			loading: false
		};
	},
	editorToolbar: null,
	props: {
		materialId: {
			type: Number,
			required: true
		},
		done: Function
	},
	computed: {
		commentRules() {
			return [
				value => !!value || this.$tl("required"),
				value =>
					htmlTextSizeOrHasImage(this.$refs?.htmlEditor?.$refs?.content, 5) ||
					this.$tl("htmlTextSizeOrHasImage")
			];
		},
		isNew: function() {
			return this.commentId == null;
		}
	},

	methods: {
		async addComment() {
			this.$refs.htmlEditor.validate();

			if (this.$refs.htmlEditor.hasError) return;

			this.loading = true;

			await this.$request(this.$Api.Comments.Create, {
				materialId: this.materialId,
				text: this.comment.text
			})
				.then(() => {
					this.$successNotify();
					this.comment.text = "";
					this.$emit("done");
					this.loading = false;
				})
				.catch(error => {
					this.$errorNotify(error);
					this.loading = false;
				});

			this.$refs.htmlEditor.resetValidation();
		}
	},
	beforeCreate() {
		this.editorToolbar = JSON.parse(config.Editor.CommentToolbar);
		this.$options.components.SunEditor = require("sun").SunEditor;
	}
};
</script>

<style lang="scss">
.create-comment__editor {
	margin-bottom: 7px;
}
</style>
