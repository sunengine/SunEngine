<template>
	<div class="edit-comment">
		<template v-if="comment">
			<SunEditor
				content-class="comment__text"
				:toolbar="editorToolbar"
				:rules="commentRules"
				class="editor"
				ref="htmlEditor"
				v-model="comment.text"
			/>

			<div>
				<q-btn
					class="send-btn"
					:icon="$iconsSet.EditComment.save"
					no-caps
					@click="updateComment"
					:loading="loading"
					:label="$tl('updateBtn')"
				>
					<LoaderSent slot="loading" />
				</q-btn>
				<q-btn
					class="cancel-btn q-ml-sm"
					no-caps
					:icon="$iconsSet.EditComment.cancel"
					@click="$emit('cancel')"
					:label="$tl('cancelBtn')"
				/>
			</div>
		</template>
		<LoaderWait v-else />
	</div>
</template>

<script>
import { htmlTextSizeOrHasImage } from "sun";

export default {
	name: "EditComment",
	data() {
		return {
			comment: null,
			loading: false
		};
	},
	props: {
		commentId: {
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
		}
	},
	methods: {
		async updateComment() {
			this.$refs.htmlEditor.validate();

			if (this.$refs.htmlEditor.hasError) {
				return;
			}
			this.loading = true;
			await this.$request(this.$Api.Comments.Update, {
				Id: this.commentId,
				MaterialId: this.comment.materialId,
				Text: this.comment.text
			})
				.then(() => {
					this.$successNotify();
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
	},
	created() {
		this.$request(this.$Api.Comments.Get, {
			id: this.commentId
		}).then(response => {
			this.comment = response.data;
		});
	}
};
</script>

<style lang="scss">
.edit-comment {
	.editor {
		margin-bottom: 7px;
	}
}
</style>
